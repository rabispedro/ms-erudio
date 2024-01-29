using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GeekShopping.CouponAPI.Models.Context;

public class MySqlContext : DbContext
{
	public MySqlContext(DbContextOptions<MySqlContext> options) : base(options) { }

	public DbSet<Coupon> Coupons { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Coupon>().HasData(
			new()
			{
				Id = 1,
				CouponCode = "ERUDIO_2023_12",
				DiscountAmount = 10.50M
			},
			new()
			{
				Id = 2,
				CouponCode = "ERUDIO_2024_01",
				DiscountAmount = 50.00M
			}
		);
	}
}
