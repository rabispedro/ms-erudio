using GeekShopping.ProductAPI.Data.ValueObjects;

namespace GeekShopping.ProductAPI.Repositories;

public interface IProductRepository
{
	Task<bool> Delete(long id);
	Task<ProductVO> Create(ProductVO productVO);
	Task<IEnumerable<ProductVO>> FindAll();
	Task<ProductVO> FindById(long id);
	Task<ProductVO> Update(ProductVO productVO);
}
