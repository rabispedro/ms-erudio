using AutoMapper;
using GeekShopping.ProductAPI.Data.ValueObjects;
using GeekShopping.ProductAPI.Models;

namespace GeekShopping.ProductAPI.Config;

public class MappingConfig
{
	public static MapperConfiguration RegisterMaps()
	{
		var MappingConfig = new MapperConfiguration(config =>
		{
			config.CreateMap<ProductVO, Product>();
			config.CreateMap<Product, ProductVO>();
		});

		return MappingConfig;
	}
}
