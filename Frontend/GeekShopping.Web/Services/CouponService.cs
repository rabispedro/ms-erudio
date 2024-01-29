using System.Net;
using System.Net.Http.Headers;
using GeekShopping.Web.Models;
using GeekShopping.Web.Services.Interfaces;
using GeekShopping.Web.Utils;

namespace GeekShopping.Web.Services;

public class CouponService : ICouponService
{
	private readonly ILogger<CouponService> _logger;
	private readonly HttpClient _httpClient;
	public const string BasePath = "api/v1/Coupon";

	public CouponService(ILogger<CouponService> logger, HttpClient httpClient)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
	}

	public async Task<CouponViewModel> FindByCode(string code, string token)
	{
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		var response = await _httpClient.GetAsync($"{BasePath}/{code}");
		if (response.StatusCode != HttpStatusCode.OK)
			return new CouponViewModel();

		return await response.ReadContentAs<CouponViewModel>();
	}
}
