namespace ExpenseOn.Repository.MySql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq.Expressions;

    /// <summary>
    ///     Provides methods for executing queries for entities of type <typeparamref name="TEntity"/> against relational databases.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that the repository operates.</typeparam>
    /// <typeparam name="TKey">The type of the entity's primary key.</typeparam>
    public interface IQueryRepository<TEntity, in TKey> where TEntity : class
    {
        bool Any(IDbTransaction transaction = null);
        bool Any(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null);
        long Count(IDbTransaction transaction = null);
        long Count(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null);
        TEntity Find(TKey key, IDbTransaction transaction = null);
        TEntity FirstOrDefault(IDbTransaction transaction = null);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null);
        IReadOnlyList<TEntity> GetAll(IDbTransaction transaction = null);
        (IReadOnlyList<TEntity> entities, long count) GetAll(int page, int pageSize, IDbTransaction transaction = null);
        IReadOnlyList<TEntity> Get(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null);
        (IReadOnlyList<TEntity> entities, long count) Get(Expression<Func<TEntity, bool>> predicate, int page, int pageSize, IDbTransaction transaction = null);
    }
}
