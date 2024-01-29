using GeekShopping.OrderDetailAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.OrderAPI.Models.Context;

public class MySqlContext : DbContext
{
	public MySqlContext(DbContextOptions<MySqlContext> options) : base(options) { }

	public DbSet<OrderDetail> OrderDetails { get; set; }
	public DbSet<OrderHeader> OrderHeaders { get; set; }
}
