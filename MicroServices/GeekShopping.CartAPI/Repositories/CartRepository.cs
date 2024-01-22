using AutoMapper;
using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShopping.CartAPI.Models;
using GeekShopping.CartAPI.Models.Context;
using GeekShopping.CartAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CartAPI.Repositories;

public class CartRepository : ICartRepository
{
	private readonly MySqlContext _context;
	private readonly IMapper _mapper;

	public CartRepository(MySqlContext context, IMapper mapper)
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
		_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
	}

	public Task<bool> ApplyCoupon(string userId, string couponCode)
	{
		throw new NotImplementedException();
	}

	public async Task<bool> Clear(string userId)
	{
		var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(item => item.UserId == userId);
		if (cartHeader == null)
			return false;

		_context.CartDetails.RemoveRange(
			_context.CartDetails.Where(item => item.CartHeaderId == cartHeader.Id));

		_context.CartHeaders.Remove(cartHeader);

		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<CartVO> FindByUserId(string id)
	{
		Cart cart = new Cart
		{
			CartHeader = await _context.CartHeaders.FirstOrDefaultAsync(item => item.UserId == id),
		};
		cart.CartDetails = _context.CartDetails
			.Where(item => item.CartHeaderId == cart.CartHeader.Id)
			.Include(item => item.Product);

		return _mapper.Map<CartVO>(cart);
	}

	public async Task<bool> RemoveCartDetail(long cartDetailId)
	{
		try
		{
			CartDetail cartDetail = await _context.CartDetails.FirstOrDefaultAsync(item => item.Id == cartDetailId);

			int total = _context.CartDetails.Where(item => item.CartHeaderId == cartDetail.CartHeaderId).Count();

			_context.CartDetails.Remove(cartDetail);
			if (total == 1)
			{
				var cartHeaderToRemove = await _context.CartHeaders.FirstOrDefaultAsync(item => item.Id == cartDetail.CartHeaderId);
				_context.CartHeaders.Remove(cartHeaderToRemove);
			}

			await _context.SaveChangesAsync();

			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public Task<bool> RemoveCoupon(string userId)
	{
		throw new NotImplementedException();
	}

	public async Task<CartVO> SaveOrUpdate(CartVO cartVo)
	{
		var cart = _mapper.Map<Cart>(cartVo);
		var product = await _context.Products.FirstOrDefaultAsync(item =>
			item.Id == cartVo.CartDetails.FirstOrDefault().ProductId);

		if (product == null)
		{
			_context.Products.Add(cart.CartDetails.FirstOrDefault().Product);
			await _context.SaveChangesAsync();
		}

		var cartHeader = await _context.CartHeaders.AsNoTracking().FirstOrDefaultAsync(item =>
			item.UserId == cart.CartHeader.UserId);

		if (cartHeader == null)
		{
			_context.CartHeaders.Add(cart.CartHeader);
			await _context.SaveChangesAsync();

			cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.Id;
			cart.CartDetails.FirstOrDefault().Product = null;
			_context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
			await _context.SaveChangesAsync();
		}
		else
		{
			var cartDetail = await _context.CartDetails.AsNoTracking().FirstOrDefaultAsync(item =>
				item.ProductId == cart.CartDetails.FirstOrDefault().ProductId && item.CartHeaderId == cartHeader.Id);

			if (cartDetail == null)
			{
				cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeader.Id;
				cart.CartDetails.FirstOrDefault().Product = null;
				_context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
				await _context.SaveChangesAsync();
			}
			else
			{
				cart.CartDetails.FirstOrDefault().Product = null;
				cart.CartDetails.FirstOrDefault().Count += cartDetail.Count;
				cart.CartDetails.FirstOrDefault().Id = cartDetail.Id;
				cart.CartDetails.FirstOrDefault().CartHeaderId = cartDetail.CartHeaderId;
				_context.CartDetails.Update(cart.CartDetails.FirstOrDefault());
				await _context.SaveChangesAsync();
			}
		}

		return _mapper.Map<CartVO>(cart);
	}
}
