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

	public async Task<ProductModel> Create(ProductModel model, string token)
	{
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		var response = await _httpClient.PostAsJson(BasePath, model);
		if (!response.IsSuccessStatusCode)
			throw new Exception("Something went wrong when calling API");

		return await response.ReadContentAs<ProductModel>();
	}

	public async Task<bool> Delete(long id, string token)
	{
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		var response = await _httpClient.DeleteAsync($"{BasePath}/{id}");
		_logger.LogInformation("Response: {response}", response);
		_logger.LogInformation("IsSuccessStatusCode: {response}", response.IsSuccessStatusCode);
		if (!response.IsSuccessStatusCode)
			throw new Exception("Something went wrong when calling API");

		bool result = await response.ReadContentAs<bool>();
		_logger.LogInformation("Result: {result}", result);

		return result;
	}

	public async Task<IEnumerable<ProductModel>> FindAll(string token)
	{
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		var response = await _httpClient.GetAsync(BasePath);
		return await response.ReadContentAs<List<ProductModel>>();
	}

	public async Task<ProductModel> FindById(long id, string token)
	{
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		var response = await _httpClient.GetAsync($"{BasePath}/{id}");
		return await response.ReadContentAs<ProductModel>();
	}

	public async Task<ProductModel> Update(ProductModel model, string token)
	{
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		var response = await _httpClient.PutAsJson(BasePath, model);
		if (!response.IsSuccessStatusCode)
			throw new Exception("Something went wrong when calling API");

		return await response.ReadContentAs<ProductModel>();
	}
}
