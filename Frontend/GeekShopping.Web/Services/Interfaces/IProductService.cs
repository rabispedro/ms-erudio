using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services.Interfaces;

public interface IProductService
{
	Task<ProductModel> Create(ProductModel model);
	Task<bool> Delete(long id);
	Task<IEnumerable<ProductModel>> FindAll();
	Task<ProductModel> FindById(long id);
	Task<ProductModel> Update(ProductModel model);
}
