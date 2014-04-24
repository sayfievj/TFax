using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace TFax.Web.CORE.BLL.Infrastructure
{
    /// <summary>
    /// Base class for all SQL based service classes
    /// </summary>
    /// <typeparam name="T">The domain object type</typeparam>
    /// <typeparam name="TU">The database object type</typeparam>
    public sealed class BaseRepository<T> : IBaseRepository<T>
        where T : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbSet = _unitOfWork.Db.Set<T>();
        }

        /// <summary>
        /// Returns the object with the primary key specifies or throws
        /// </summary>
        /// <typeparam name="TU">The type to map the result to</typeparam>
        /// <param name="primaryKey">The primary key</param>
        /// <returns>The result mapped to the specified type</returns>
        public T Find(object primaryKey)
        {
            return _dbSet.Find(primaryKey);
        }

        public DbSet<T> All
        {
            get { return _dbSet; }
        }

        public IQueryable<T> FindAll(Expression<Func<T, bool>> predicate = null)
        {
            return _dbSet.Where(predicate);
        }

        public T Find(Expression<Func<T, bool>> predicate = null)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public bool Exists(object primaryKey)
        {
            return Find(primaryKey) != null;
        }

        public bool Any(Expression<Func<T, bool>> predicate = null)
        {
            return _dbSet.Any(predicate);
        }

        public long Insert(T entity)
        {
            dynamic t = _dbSet.Add(entity);

            _unitOfWork.Db.Entry(entity).State = EntityState.Added;
            _unitOfWork.Db.SaveChanges();

            return (long)t.Id;
        }

        public T Update(T entity)
        {
            dynamic t = _dbSet.Attach(entity);

            _unitOfWork.Db.Entry(entity).State = EntityState.Modified;
            _unitOfWork.Db.SaveChanges();

            return t;
        }

        public long Delete(T entity)
        {
            if (_unitOfWork.Db.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            dynamic t = _dbSet.Remove(entity);

            _unitOfWork.Db.Entry(entity).State = EntityState.Deleted;
            _unitOfWork.Db.SaveChanges();

            return (long)t.Id;
        }

        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.AsEnumerable();
        }

    }
}
