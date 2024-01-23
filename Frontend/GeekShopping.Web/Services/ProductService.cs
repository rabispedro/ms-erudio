using System.Net.Http.Headers;
using GeekShopping.Web.Models;
using GeekShopping.Web.Services.Interfaces;
using GeekShopping.Web.Utils;

namespace GeekShopping.Web.Services;

public class ProductService : IProductService
{
	private readonly ILogger<ProductService> _logger;
	private readonly HttpClient _httpClient;

	public const string BasePath = "api/v1/Product";

	public ProductService(ILogger<ProductService> logger, HttpClient httpClient)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
	}

	public async Task<ProductViewModel> Create(ProductViewModel model, string token)
	{
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		var response = await _httpClient.PostAsJson(BasePath, model);
		if (!response.IsSuccessStatusCode)
			throw new Exception("Something went wrong when calling ProductAPI");

		return await response.ReadContentAs<ProductViewModel>();
	}

	public async Task<bool> Delete(long id, string token)
	{
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		var response = await _httpClient.DeleteAsync($"{BasePath}/{id}");
		if (!response.IsSuccessStatusCode)
			throw new Exception("Something went wrong when calling ProductAPI");

		return await response.ReadContentAs<bool>();
	}

	public async Task<IEnumerable<ProductViewModel>> FindAll(string token)
	{
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		var response = await _httpClient.GetAsync(BasePath);
		return await response.ReadContentAs<List<ProductViewModel>>();
	}

	public async Task<ProductViewModel> FindById(long id, string token)
	{
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		var response = await _httpClient.GetAsync($"{BasePath}/{id}");
		return await response.ReadContentAs<ProductViewModel>();
	}

	public async Task<ProductViewModel> Update(ProductViewModel model, string token)
	{
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		var response = await _httpClient.PutAsJson(BasePath, model);
		if (!response.IsSuccessStatusCode)
			throw new Exception("Something went wrong when calling ProductAPI");

		return await response.ReadContentAs<ProductViewModel>();
	}
}
