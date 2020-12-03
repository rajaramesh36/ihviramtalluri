namespace InstaHangouts.Dal.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Interface IBaseRepository
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <seealso cref="System.IDisposable" />
    public interface IBaseRepository<TEntity> : IDisposable
    {
        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Add(TEntity entity);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="propertyNames">The property names.</param>
        void Update(TEntity entity, params object[] propertyNames);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Deletes the where.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        void DeleteWhere(Func<TEntity, bool> predicate);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>All the Records from entity</returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Exists the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>return bool value</returns>
        bool Exists(Func<TEntity, bool> predicate);

        /// <summary>
        /// Finds the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>returns the Entity.</returns>
        TEntity Find(Func<TEntity, bool> predicate);

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>returns the list of entity records</returns>
        IEnumerable<TEntity> FindAll(Func<TEntity, bool> predicate);
    }
}
