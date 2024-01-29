using GeekShopping.Web.Models;
using GeekShopping.Web.Services.Interfaces;

namespace GeekShopping.Web.Services;

public class CouponService : ICouponService
{
	public Task<CouponViewModel> FindByCode(string code)
	{
		throw new NotImplementedException();
	}
}
