using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShopping.CartAPI.Models.Context;
using GeekShopping.CartAPI.Repositories.Interface;

namespace GeekShopping.CartAPI.Repositories;

public class CouponRepository : ICouponRepository
{
	private readonly HttpClient _httpClient;
	public const string BasePath = "api/v1/Coupon";


	public CouponRepository(HttpClient httpClient)
	{
		_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
	}

	public async Task<CouponVO> FindByCode(string code, string token)
	{
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		var response = await _httpClient.GetAsync($"{BasePath}/coupon/{code}");
		if (response.StatusCode != HttpStatusCode.OK)
			return new CouponVO();

		var content = await response.Content.ReadAsStringAsync();

		return JsonSerializer.Deserialize<CouponVO>(content);
	}
}
