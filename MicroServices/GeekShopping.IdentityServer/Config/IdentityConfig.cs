using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace GeekShopping.IdentityServer.Config;

public static class IdentityConfig
{
	public const string Admin = "Admin";
	public const string Client = "Client";

	public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
	{
		new IdentityResources.OpenId(),
		new IdentityResources.Email(),
		new IdentityResources.Profile()
	};

	public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
	{
		new("geek_shopping", "GeekShopping Server"),
		new(name: "read", "Read Data."),
		new(name: "write", "Write Data."),
		new(name: "delete", "Delete Data."),
	};

	public static IEnumerable<Client> Clients => new List<Client>
	{
		new()
		{
			ClientId = "client",
			ClientSecrets = { new Secret("IDontRecommendUseThisSecretMethod".Sha256()) },
			AllowedGrantTypes = GrantTypes.ClientCredentials,
			AllowedScopes = { "read", "write", "profile" }
		},
		new()
		{
			ClientId = "geek_shopping",
			ClientSecrets = { new Secret("IDontRecommendUseThisSecretMethod".Sha256()) },
			AllowedGrantTypes = GrantTypes.Code,
			RedirectUris = { "https://localhost:7030/signin-oidc" },
			PostLogoutRedirectUris = { "https://localhost:7030/signout-callback-oidc" },
			AllowedScopes =
			{
				IdentityServerConstants.StandardScopes.OpenId,
				IdentityServerConstants.StandardScopes.Profile,
				IdentityServerConstants.StandardScopes.Email,
				"geek_shopping"
			},
		},
	};
}
