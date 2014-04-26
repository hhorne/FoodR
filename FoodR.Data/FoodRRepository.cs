using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FoodR.Data
{
	public class FoodRRepository : IRepository
	{
		private bool disposed = false;
		private bool isDisposing;

		private readonly FoodRContext context;

		public FoodRRepository(FoodRContext context)
		{
			this.context = context;
		}

		public IQueryable<T> GetAll<T>() where T : class
		{
			var result = context.Set<T>();
			return result;
		}

		public IEnumerable<T> Where<T>(Func<T, bool> predicate) where T : class
		{
			return context.Set<T>().Where(predicate);
		}

		public void Delete<T>(T entity) where T : class
		{
			context.Set<T>().Remove(entity);
		}

		public void Add<T>(T entity) where T : class
		{
			context.Set<T>().Add(entity);
		}

		public void SaveChanges()
		{
			context.SaveChanges();
		}

		public bool IsDisposed()
		{
			return disposed;
		}

		public void Dispose()
		{
			if (!IsDisposed())
			{
				context.Dispose();
				this.disposed = true;
			}
		}
	}
	
	public interface IRepository : IDisposable
	{
		IQueryable<T> GetAll<T>() where T : class;
		IEnumerable<T> Where<T>(Func<T, bool> predicate) where T : class;
		void Delete<T>(T entity) where T : class;
		void Add<T>(T entity) where T : class;
		void SaveChanges();
		bool IsDisposed();
	}
}