using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddAuthentication("Bearer")
	.AddJwtBearer("Bearer", options =>
	{
		options.Authority = "http://localhost:5021/";
		options.RequireHttpsMetadata = false;
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateAudience = false
		};
	});

builder.Services.AddOcelot();

var app = builder.Build();

app.UseOcelot();

app.Run();
