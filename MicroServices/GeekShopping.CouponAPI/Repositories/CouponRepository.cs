using AutoMapper;
using GeekShopping.CouponAPI.Models.Context;
using GeekShopping.CouponAPI.Repositories.Interfaces;
using GeekShopping.CouponVOAPI.Data.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CouponAPI.Repositories;

public class CouponRepository : ICouponRepository
{
	private readonly MySqlContext _context;
	private readonly IMapper _mapper;

	public CouponRepository(MySqlContext context, IMapper mapper)
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
		_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
	}

	public async Task<CouponVO> FindByCode(string code)
	{
		var coupon = await _context.Coupons.FirstOrDefaultAsync(item => item.Code == code);
		return _mapper.Map<CouponVO>(coupon);
	}
}
