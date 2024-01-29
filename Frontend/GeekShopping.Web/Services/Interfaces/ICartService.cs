using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services.Interfaces;

public interface ICartService
{
	Task<CartViewModel> FindByUserId(string id, string token);
	Task<CartViewModel> AddItem(CartViewModel model, string token);
	Task<CartViewModel> Update(CartViewModel model, string token);
	Task<bool> RemoveItem(long cartId, string token);
	Task<bool> ApplyCoupon(CartViewModel model, string token);
	Task<bool> RemoveCoupon(string userId, string token);
	Task<bool> Clear(string userId, string token);
	Task<object> Checkout(CartHeaderViewModel model, string token);
}
