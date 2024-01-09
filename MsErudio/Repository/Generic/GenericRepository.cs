
using Microsoft.EntityFrameworkCore;
using MsErudio.Model.Base;
using MsErudio.Model.Context;
using Pomelo.EntityFrameworkCore.MySql.Metadata.Internal;

namespace MsErudio.Repository.Generic;

public class GenericRepository<T> : IRepository<T> where T : BaseEntity
{
	private MySQLContext _context;
	private DbSet<T> dataset;

	public GenericRepository(MySQLContext context)
	{
		_context = context;
		dataset = _context.Set<T>();
	}

	public List<T> FindAll()
	{
		return dataset.ToList();
	}

	public T FindById(long id)
	{
		return dataset.SingleOrDefault(p => p.Id.Equals(id));
	}

	public T Create(T item)
	{
		try
		{
			dataset.Add(item);
			_context.SaveChanges();
			return item;
		}
		catch (Exception)
		{
			throw;
		}
	}

	public T Update(T item)
	{
		var result = dataset.SingleOrDefault(p => p.Id.Equals(item.Id));
		if (result == null)
			return null;

		try
		{
			_context.Entry(result).CurrentValues.SetValues(item);
			_context.SaveChanges();
			return result;
		}
		catch (Exception)
		{
			throw;
		}
	}

	public void Delete(long id)
	{
		var result = dataset.SingleOrDefault(p => p.Id.Equals(id));
		if (result == null)
			return;

		try
		{
			dataset.Remove(result);
			_context.SaveChanges();
		}
		catch (Exception)
		{
			throw;
		}
	}

	public bool Exists(long id)
	{
		return dataset.Any(p => p.Id.Equals(id));
	}
}
