using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services.Interfaces;

public interface ICouponService
{
	Task<CouponViewModel> FindByCode(string code, string token);
}
