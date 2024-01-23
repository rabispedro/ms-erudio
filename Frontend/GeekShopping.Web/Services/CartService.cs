using System.Net.Http.Headers;
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
		// _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		Console.WriteLine($"Model: {model}\n\nToken: {token}");
		// var response = await _httpClient.PostAsJson($"{BasePath}/add-cart", model);
		// // var response = await _httpClient.PostAsJson(BasePath, model);

		// if (!response.IsSuccessStatusCode)
		// 	throw new Exception("Something went wrong when calling CartAPI");

		// return await response.ReadContentAs<CartViewModel>();

		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		var response = await _httpClient.PostAsJson($"{BasePath}/add-cart", model);
		if (response.IsSuccessStatusCode)
			return await response.ReadContentAs<CartViewModel>();
		else throw new Exception("Something went wrong when calling API");
	}

	public Task<bool> ApplyCoupon(CartViewModel model, string couponCode, string token)
	{
		throw new NotImplementedException();
	}

	public Task<CartViewModel> Checkout(CartHeaderViewModel model, string token)
	{
		throw new NotImplementedException();
	}

	public Task<bool> Clear(string userId, string token)
	{
		throw new NotImplementedException();
	}

	public async Task<CartViewModel> FindByUserId(string id, string token)
	{
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		Console.WriteLine($"Id: {id}");
		var response = await _httpClient.GetAsync($"{BasePath}/find-cart/{id}");
		return await response.ReadContentAs<CartViewModel>();
	}

	public Task<bool> RemoveCoupon(string userId, string token)
	{
		throw new NotImplementedException();
	}

	public async Task<bool> RemoveItem(long cartId, string token)
	{
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		Console.WriteLine($"CartId: {cartId}");
		var response = await _httpClient.DeleteAsync($"{BasePath}/remove-cart/{cartId}");
		if (!response.IsSuccessStatusCode)
			throw new Exception("Something went wrong when calling CartAPI");

		return await response.ReadContentAs<bool>();
	}

	public async Task<CartViewModel> Update(CartViewModel model, string token)
	{
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		Console.WriteLine($"Model: {model}");
		var response = await _httpClient.PutAsJson($"{BasePath}/update-cart/", model);
		if (!response.IsSuccessStatusCode)
			throw new Exception("Something went wrong when calling CartAPI");

		return (await response.ReadContentAs<CartViewModel>());
	}
}
