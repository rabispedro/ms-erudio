using GeekShopping.OrderAPI.Models;

namespace GeekShopping.OrderAPI.Repositories.Interfaces;

public interface IOrderRepository
{
	Task<bool> AddOrder(OrderHeader orderHeader);
	Task<bool> UpdateOrderPaymentStatus(long orderHeaderId, bool paymentStatus);
}
