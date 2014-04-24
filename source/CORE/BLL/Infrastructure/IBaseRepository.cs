using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace TFax.Web.CORE.BLL.Infrastructure
{
    public interface IBaseRepository<T> where T : class
    {

        /// <summary>
        /// Retrieve a single item using it's primary key, exception if not found
        /// </summary>
        /// <param name="primaryKey">The primary key of the record</param>
        /// <returns>T</returns>
        T Find(object primaryKey);

        DbSet<T> All { get; }

        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate = null);

        T Find(Expression<Func<T, bool>> predicate = null);

        /// <summary>
        /// Returns all the rows for type T
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Does this item exist by it's primary key
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        bool Exists(object primaryKey);

        bool Any(Expression<Func<T, bool>> predicate=null);

        /// <summary>
        /// Inserts the data into the table
        /// </summary>
        /// <param name="entity">The entity to insert</param>
        /// <param>The user performing the insert</param>
        /// <returns></returns>
        long Insert(T entity);

        /// <summary>
        /// Updates this entity in the database using it's primary key
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <param name="userId">The user performing the update</param>
        T Update(T entity);

        /// <summary>
        /// Deletes this entry fro the database
        /// ** WARNING - Most items should be marked inactive and Updated, not deleted
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        /// <param>The user Id who deleted the entity</param>
        /// <returns></returns>
        long Delete(T entity);

        IUnitOfWork UnitOfWork { get; }

    }
}