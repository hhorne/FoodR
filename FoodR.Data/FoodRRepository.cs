using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FoodR.Data
{
	public class FoodRRepository : IRepository
	{
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

		public T Find<T>(int id) where T : class
		{
			var result = Find<T>(new object[] {id});
			return result;
		}

		public T Find<T>(object[] keyValues) where T : class
		{
			var result = context.Set<T>().Find(keyValues);
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

		protected virtual void Dispose(bool managed)
		{
			if(managed)
			{
				if (context != null)
				{
					context.Dispose();
				}
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
	
	public interface IRepository : IDisposable
	{
		IQueryable<T> GetAll<T>() where T : class;
		T Find<T>(int id) where T : class;
		T Find<T>(object[] keyValues) where T : class;
		IEnumerable<T> Where<T>(Func<T, bool> predicate) where T : class;
		void Delete<T>(T entity) where T : class;
		void Add<T>(T entity) where T : class;
		void SaveChanges();
	}
}