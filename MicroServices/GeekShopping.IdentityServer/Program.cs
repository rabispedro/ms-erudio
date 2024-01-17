using GeekShopping.IdentityServer.Config;
using GeekShopping.IdentityServer.Initializer;
using GeekShopping.IdentityServer.Initializer.Interfaces;
using GeekShopping.IdentityServer.Model;
using GeekShopping.IdentityServer.Model.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MySqlContext>(options => options.UseMySql(
	builder.Configuration["MySqlConnection:MySqlConnectionString"],
	new MySqlServerVersion(new Version(8, 2, 0))
));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
	.AddEntityFrameworkStores<MySqlContext>()
	.AddDefaultTokenProviders();

var builderIdentityService = builder.Services.AddIdentityServer(options =>
{
	options.Events.RaiseErrorEvents = true;
	options.Events.RaiseInformationEvents = true;
	options.Events.RaiseFailureEvents = true;
	options.Events.RaiseSuccessEvents = true;
	options.EmitStaticAudienceClaim = true;
})
	.AddInMemoryIdentityResources(IdentityConfig.IdentityResources)
	.AddInMemoryApiScopes(IdentityConfig.ApiScopes)
	.AddInMemoryClients(IdentityConfig.Clients)
	.AddAspNetIdentity<ApplicationUser>();

builder.Services.AddScoped<IDbInitializer, DbInitializer>();

builderIdentityService.AddDeveloperSigningCredential();

var app = builder.Build();
var initializer = app.Services.CreateScope().ServiceProvider.GetService<IDbInitializer>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
		app.UseExceptionHandler("/Home/Error");
		// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
		app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();

initializer.Initialize();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
