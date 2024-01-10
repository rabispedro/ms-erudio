using AutoMapper;
using GeekShopping.ProductAPI.Data.ValueObjects;
using GeekShopping.ProductAPI.Models;
using GeekShopping.ProductAPI.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.ProductAPI.Repositories;

public class ProductRepository : IProductRepository
{
	private readonly MySqlContext _context;
	private readonly IMapper _mapper;

	public ProductRepository() {}

	public ProductRepository(MySqlContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<ProductVO> Create(ProductVO productVO)
	{
		var product = _mapper.Map<Product>(productVO);
		_context.Products.Add(product);
		await _context.SaveChangesAsync();
		return _mapper.Map<ProductVO>(product);
	}

	public async Task<bool> Delete(long id)
	{
		try
		{
			var product = await _context.Products.Where(item => item.Id == id).FirstOrDefaultAsync() ?? new Product();
			if (product.Id <= 0)
				return false;

			_context.Products.Remove(product);
			await _context.SaveChangesAsync();

			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public async Task<IEnumerable<ProductVO>> FindAll()
	{
		var products = await _context.Products.ToListAsync();
		return _mapper.Map<List<ProductVO>>(products);
	}

	public async Task<ProductVO> FindById(long id)
	{
		var product = await _context.Products.Where(item => item.Id == id).FirstOrDefaultAsync() ?? new Product();
		return _mapper.Map<ProductVO>(product);
	}

	public async Task<ProductVO> Update(ProductVO productVO)
	{
		var product = _mapper.Map<Product>(productVO);
		_context.Products.Update(product);
		await _context.SaveChangesAsync();
		return _mapper.Map<ProductVO>(product);
	}
}
