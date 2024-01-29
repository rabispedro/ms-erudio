using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks.Dataflow;
using GeekShopping.Web.Models;
using GeekShopping.Web.Services.Interfaces;
using GeekShopping.Web.Utils;

namespace GeekShopping.Web.Services;

public class CartService : ICartService
{
	private readonly ILogger<CartService> _logger;
	private readonly HttpClient _httpClient;
	public const string BasePath = "api/v1/Cart";

	public CartService(ILogger<CartService> logger, HttpClient httpClient)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
	}

	public async Task<CartViewModel> AddItem(CartViewModel model, string token)
	{
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		var response = await _httpClient.PostAsJson($"{BasePath}/add-cart", model);
		if (!response.IsSuccessStatusCode)
			throw new Exception("Something went wrong when calling Cart API");

		return await response.ReadContentAs<CartViewModel>();
	}

	public async Task<bool> ApplyCoupon(CartViewModel model, string token)
	{
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		var response = await _httpClient.PostAsJson($"{BasePath}/apply-coupon", model);
		if (!response.IsSuccessStatusCode)
			throw new Exception("Something went wrong when calling Cart API");

		return await response.ReadContentAs<bool>();
	}

	public async Task<object> Checkout(CartHeaderViewModel model, string token)
	{
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		var response = await _httpClient.PostAsJson($"{BasePath}/checkout", model);
		if (!response.IsSuccessStatusCode)
		{
			if (response.StatusCode != HttpStatusCode.PreconditionFailed)
				throw new Exception("Something went wrong when calling Cart API");

			return "Coupon price has changed, please confirm!";
		}

		return await response.ReadContentAs<CartHeaderViewModel>();
	}

	public Task<bool> Clear(string userId, string token)
	{
		throw new NotImplementedException();
	}

	public async Task<CartViewModel> FindByUserId(string id, string token)
	{
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		var response = await _httpClient.GetAsync($"{BasePath}/find-cart/{id}");
		return await response.ReadContentAs<CartViewModel>();
	}

	public async Task<bool> RemoveCoupon(string userId, string token)
	{
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		var response = await _httpClient.DeleteAsync($"{BasePath}/remove-coupon/{userId}");
		if (!response.IsSuccessStatusCode)
			throw new Exception("Something went wrong when calling Cart API");

		return await response.ReadContentAs<bool>();
	}

	public async Task<bool> RemoveItem(long cartId, string token)
	{
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		var response = await _httpClient.DeleteAsync($"{BasePath}/remove-cart/{cartId}");
		if (!response.IsSuccessStatusCode)
			throw new Exception("Something went wrong when calling Cart API");

		return await response.ReadContentAs<bool>();
	}

	public async Task<CartViewModel> Update(CartViewModel model, string token)
	{
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		var response = await _httpClient.PutAsJson($"{BasePath}/update-cart/", model);
		if (!response.IsSuccessStatusCode)
			throw new Exception("Something went wrong when calling Cart API");

		return await response.ReadContentAs<CartViewModel>();
	}
}
