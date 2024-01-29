using GeekShopping.OrderAPI.MessageConsumer;
using GeekShopping.OrderAPI.Models.Context;
using GeekShopping.OrderAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


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

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("ApiScope", policy =>
	{
		policy.RequireAuthenticatedUser();
		policy.RequireClaim("scope", "geek_shopping_web");
	});
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo { Title = "GeekShopping.OrderAPI", Version = "v1" });
	options.EnableAnnotations();
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = @"Enter 'Bearer' [space] and your token",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer",
	});
	options.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer",
				},
				Scheme = "oauth2",
				Name = "Bearer",
				In = ParameterLocation.Header
			},
			new List<string>()
		}
	});
});

builder.Services.AddDbContext<MySqlContext>(options =>
	options.UseMySql(
		builder.Configuration["MySqlConnection:MySqlConnectionString"],
		new MySqlServerVersion(new Version(8, 2, 0))
));

var dbContextBuilder = new DbContextOptionsBuilder<MySqlContext>();
dbContextBuilder.UseMySql(
	builder.Configuration["MySqlConnection:MySqlConnectionString"],
	new MySqlServerVersion(new Version(8, 2, 0))
);

builder.Services.AddSingleton(new OrderRepository(dbContextBuilder.Options));

builder.Services.AddHostedService<RabbitMqCheckoutConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
