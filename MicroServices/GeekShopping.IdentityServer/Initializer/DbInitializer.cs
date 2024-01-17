using System.Security.Claims;
using GeekShopping.IdentityServer.Config;
using GeekShopping.IdentityServer.Initializer.Interfaces;
using GeekShopping.IdentityServer.Model;
using GeekShopping.IdentityServer.Model.Context;
using IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace GeekShopping.IdentityServer.Initializer;

public class DbInitializer : IDbInitializer
{
	private readonly MySqlContext _context;
	private readonly UserManager<ApplicationUser> _user;
	private readonly RoleManager<IdentityRole> _role;

	public DbInitializer(MySqlContext context, UserManager<ApplicationUser> user, RoleManager<IdentityRole> role)
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
		_user = user ?? throw new ArgumentNullException(nameof(user));
		_role = role ?? throw new ArgumentNullException(nameof(role));
	}
	
	public void Initialize()
	{
		if (_role.FindByNameAsync(IdentityConfig.Admin).Result != null)
			return;
		
		_role.CreateAsync(new IdentityRole(IdentityConfig.Admin)).GetAwaiter().GetResult();
		_role.CreateAsync(new IdentityRole(IdentityConfig.Client)).GetAwaiter().GetResult();

		var admin = new ApplicationUser()
		{
			UserName = "system-admin",
			Email = "system-admin@gmail.com.br",
			EmailConfirmed = true,
			PhoneNumber = "+55 (98) 99755-9399",
			FirstName = "System",
			LastName = "Admin"
		};

		_user.CreateAsync(admin, "Sys-Admin@123").GetAwaiter().GetResult();
		_user.AddToRoleAsync(admin, IdentityConfig.Admin).GetAwaiter().GetResult();

		var adminClaims = _user.AddClaimsAsync(admin, new Claim[]
		{
			new(JwtClaimTypes.Name, $"{admin.FirstName} {admin.LastName}"),
			new(JwtClaimTypes.GivenName, admin.FirstName),
			new(JwtClaimTypes.FamilyName, admin.LastName),
			new(JwtClaimTypes.Role, IdentityConfig.Admin),
		}).Result;

		var client = new ApplicationUser()
		{
			UserName = "system-client",
			Email = "system-client@gmail.com.br",
			EmailConfirmed = true,
			PhoneNumber = "+55 (98) 98270-1698",
			FirstName = "System",
			LastName = "Client"
		};

		_user.CreateAsync(client, "Sys-Client@321").GetAwaiter().GetResult();
		_user.AddToRoleAsync(client, IdentityConfig.Client).GetAwaiter().GetResult();

		var clientClaims = _user.AddClaimsAsync(client, new Claim[]
		{
			new(JwtClaimTypes.Name, $"{client.FirstName} {client.LastName}"),
			new(JwtClaimTypes.GivenName, client.FirstName),
			new(JwtClaimTypes.FamilyName, client.LastName),
			new(JwtClaimTypes.Role, IdentityConfig.Client),
		}).Result;
	}
}
