using GeekShopping.CartAPI.Data.ValueObjects;

namespace GeekShopping.CartAPI.Repositories.Interface;

public interface ICartRepository
{
	Task<CartVO> FindByUserId(string id);
	Task<CartVO> SaveOrUpdate(CartVO cartVo);
	Task<bool> RemoveCartDetail(long cartDetailId);
	Task<bool> ApplyCoupon(string userId, string couponCode);
	Task<bool> RemoveCoupon(string userId);
	Task<bool> Clear(string userId);
}
