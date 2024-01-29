using AutoMapper;
using GeekShopping.CouponAPI.Models;
using GeekShopping.CouponVOAPI.Data.ValueObjects;

namespace GeekShopping.CouponAPI.Configs;

public static class MappingConfig
{
	public static MapperConfiguration RegisterMaps()
	{
		var mappingConfig = new MapperConfiguration(config =>
		{
			config.CreateMap<CouponVO, Coupon>().ReverseMap();
		});

		return mappingConfig;
	}
}
