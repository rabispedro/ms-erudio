using System.Net;
using GeekShopping.Web.Services;
using GeekShopping.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IProductService, ProductService>(
	client => client.BaseAddress = new Uri(builder.Configuration["MicroServicesUrl:ProductAPI"])
);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(options =>
{
	options.DefaultScheme = "Cookies";
	options.DefaultChallengeScheme = "oidc";
})
	.AddCookie("Cookies", c => c.ExpireTimeSpan = TimeSpan.FromMinutes(10))
	.AddOpenIdConnect("oidc", options =>
	{
		options.Authority = builder.Configuration["MicroServicesUrl:IdentityServer"];
		options.GetClaimsFromUserInfoEndpoint = true;
		options.ClientId = "geek_shopping";
		options.ClientSecret = "IDontRecommendUseThisSecretMethod";
		options.ResponseType = "code";
		options.ClaimActions.MapJsonKey("role", "role", "role");
		options.ClaimActions.MapJsonKey("sub", "sub", "sub");
		options.TokenValidationParameters.NameClaimType = "name";
		options.TokenValidationParameters.RoleClaimType = "role";
		options.Scope.Add("geek_shopping");
		options.SaveTokens = true;
		options.RequireHttpsMetadata = false;
	});

IdentityModelEventSource.ShowPII = true;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();