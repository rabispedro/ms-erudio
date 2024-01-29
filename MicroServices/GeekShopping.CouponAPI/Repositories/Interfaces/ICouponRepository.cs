using GeekShopping.CouponVOAPI.Data.ValueObjects;

namespace GeekShopping.CouponAPI.Repositories.Interfaces;

public interface ICouponRepository
{
	Task<CouponVO> FindByCode(string code);
}
