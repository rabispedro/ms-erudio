using AutoMapper;
using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShopping.CartAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CartAPI.Configs;

public static class MappingConfig
{
	public static MapperConfiguration RegisterMaps()
	{
		var mappingConfig = new MapperConfiguration(config =>
		{
			config.CreateMap<ProductVO, Product>().ReverseMap();
			config.CreateMap<CartHeaderVO, CartHeader>().ReverseMap();
			config.CreateMap<CartDetailVO, CartDetail>().ReverseMap();
			config.CreateMap<CartVO, Cart>().ReverseMap();
		});

		return mappingConfig;
	}
}
