using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MsErudio.Model.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connection = builder.Configuration["MySQLConnection:MySQLConnectionString"];
builder.Services.AddDbContext<MySQLContext>(options =>
	options.UseMySql(connection, new MySqlServerVersion(new Version(5, 7, 44)))
);

// builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
// 	.AddEntityFrameworkStores<MySQLContext>()
// 	.AddDefaultTokenProviders();

// var builderIdentity = builder.Services.AddIdentityServer(options =>
// {
// 	options.Events.RaiseErrorEvents = true;
// 	options.Events.RaiseInformationEvents = true;
// 	options.Events.RaiseFailureEvents = true;
// 	options.Events.RaiseSuccessEvents = true;
// 	options.EmitStaticAudienceClaim = true;
// })
// 	.AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
// 	.AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
// 	.AddInMemoryClients(IdentityConfiguration.Clients)
// 	.AddAspNetIdentity<ApplicationUser>();

// builder.Services.AddScoped<IDbInitializer, DbInitializer>();

// buildeIdentity.AddDevelopersSigningCredential();

builder.Services.AddControllersWithViews();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
var scope = app.Services.CreateScope();

// var dbInitializer = scope.ServiceProvider.GetService<IDbInitializer>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
	app.UseExceptionHandler("/Home/Error");
}

// app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseApiVersioning();
// app.UseIdentityServer();
app.UseAuthorization();

// dbInitializer.Initialize();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
