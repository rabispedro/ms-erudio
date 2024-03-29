using System.ComponentModel.DataAnnotations;
using IdentityServerHost.Quickstart.UI;

namespace GeekShopping.IdentityServer.MainModule.Account;

public class RegisterViewModel
{
	[Required]
	public string Username { get; set; }
	[Required]
	public string Email { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }

	[Required]
	public string Password { get; set; }
	public string ReturnUrl { get; set; }
	public string RoleName { get; set; }
	public bool AllowRememberLogin { get; set; }
	public bool EnableLocalLogin { get; set; }
	public IEnumerable<ExternalProvider> ExternalProviders { get; set; } = Enumerable.Empty<ExternalProvider>();
	public IEnumerable<ExternalProvider> VisibleExternalProviders => ExternalProviders.Where(provider => !String.IsNullOrWhiteSpace(provider.DisplayName));
	public bool IsExternalLoginOnly => !EnableLocalLogin && ExternalProviders?.Count() == 1;
	public string ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;
}
