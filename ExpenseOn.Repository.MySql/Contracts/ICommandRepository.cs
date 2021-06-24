namespace ExpenseOn.Repository.MySql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq.Expressions;

    /// <summary>
    ///     Provides methods for executing commands for entities of type <typeparamref name="TEntity"/> against relational databases.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that the repository operates.</typeparam>
    /// <typeparam name="TKey">The type of the entity's primary key.</typeparam>
    public interface ICommandRepository<TEntity, out TKey> where TEntity : class
    {
        TKey Insert(TEntity entity, IDbTransaction transaction = null);
        void InsertMany(IEnumerable<TEntity> entities, IDbTransaction transaction = null);
        bool Update(TEntity entity, IDbTransaction transaction = null);
        void UpdateMany(IEnumerable<TEntity> entities, IDbTransaction transaction = null);
        TKey Upsert(TEntity entity, IDbTransaction transaction = null);
        void UpsertMany(IEnumerable<TEntity> entities, IDbTransaction transaction = null);
        bool Delete(TEntity entity, IDbTransaction transaction = null);
        void DeleteMany(IEnumerable<TEntity> entities, IDbTransaction transaction = null);
        bool DeleteMany(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null);
    }
}
