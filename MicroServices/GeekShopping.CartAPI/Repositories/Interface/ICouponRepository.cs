using GeekShopping.CartAPI.Data.ValueObjects;

namespace GeekShopping.CartAPI.Repositories.Interface;

public interface ICouponRepository
{
	Task<CouponVO> FindByCode(string code, string token);
}
