using GeekShopping.OrderAPI.Models;
using GeekShopping.OrderAPI.Models.Context;
using GeekShopping.OrderAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.OrderAPI.Repositories;

public class OrderRepository : IOrderRepository
{
	private readonly DbContextOptions<MySqlContext> _context;

	public OrderRepository(DbContextOptions<MySqlContext> context)
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
	}
	public async Task<bool> AddOrder(OrderHeader orderHeader)
	{
		if (orderHeader == null)
			return false;
		
		await using var _db = new MySqlContext(_context);
		_db.OrderHeaders.Add(orderHeader);

		await _db.SaveChangesAsync();

		return true;
	}

	public async Task<bool> UpdateOrderPaymentStatus(long orderHeaderId, bool paymentStatus)
	{
		if (orderHeaderId == null)
			return false;

		await using var _db = new MySqlContext(_context);
		var orderHeader = await _db.OrderHeaders.FirstOrDefaultAsync(item => item.Id == orderHeaderId);
		if (orderHeader == null)
			return false;

		orderHeader.PaymentStatus = paymentStatus;
		await _db.SaveChangesAsync();

		return true;
	}
}
