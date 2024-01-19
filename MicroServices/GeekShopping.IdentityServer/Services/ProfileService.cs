using System.Security.Claims;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using GeekShopping.IdentityServer.Model;
using IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace GeekShopping.IdentityServer.Services;

public class ProfileService : IProfileService
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
	private readonly RoleManager<IdentityRole> _roleManager;

	public ProfileService(
		UserManager<ApplicationUser> userManager,
		IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
		RoleManager<IdentityRole> roleManager)
	{
		_userManager = userManager;
		_userClaimsPrincipalFactory = userClaimsPrincipalFactory;
		_roleManager = roleManager;
	}

	public async Task GetProfileDataAsync(ProfileDataRequestContext context)
	{
		string id = context.Subject.GetSubjectId();
		ApplicationUser user = await _userManager.FindByIdAsync(id);
		ClaimsPrincipal userClaims = await _userClaimsPrincipalFactory.CreateAsync(user);

		List<Claim> claims = userClaims.Claims.ToList();
		claims.Add(new(JwtClaimTypes.FamilyName, user.LastName));
		claims.Add(new(JwtClaimTypes.GivenName, user.FirstName));

		if (_userManager.SupportsUserRole)
		{
			IList<string> roles = await _userManager.GetRolesAsync(user);
			foreach (var role in roles)
			{
				claims.Add(new(JwtClaimTypes.Role, role));

				if (_roleManager.SupportsRoleClaims)
				{
					IdentityRole identityRole = await _roleManager.FindByNameAsync(role);

					if (identityRole != null)
					{
						claims.AddRange(await _roleManager.GetClaimsAsync(identityRole));
					}
				}
			}
		}

		context.IssuedClaims = claims;
	}

	public async Task IsActiveAsync(IsActiveContext context)
	{
		string id = context.Subject.GetSubjectId();
		ApplicationUser user = await _userManager.FindByIdAsync(id);
		context.IsActive = user != null;
	}
}
