namespace ExpenseOn.Repository.MySql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    ///     Provides asynchronous methods for executing commands for entities of type <typeparamref name="TEntity"/> against relational databases.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that the repository operates.</typeparam>
    /// <typeparam name="TKey">The type of the entity's primary key.</typeparam>
    public interface ICommandRepositoryAsync<TEntity, TKey> where TEntity : class
    {
        Task<TKey> InsertAsync(TEntity entity, IDbTransaction transaction = null);
        Task InsertManyAsync(IEnumerable<TEntity> entities, IDbTransaction transaction = null);
        Task<bool> UpdateAsync(TEntity entity, IDbTransaction transaction = null);
        Task UpdateManyAsync(IEnumerable<TEntity> entities, IDbTransaction transaction = null);
        Task<TKey> UpsertAsync(TEntity entity, IDbTransaction transaction = null);
        Task UpsertManyAsync(IEnumerable<TEntity> entities, IDbTransaction transaction = null);
        Task<bool> DeleteAsync(TEntity entity, IDbTransaction transaction = null);
        Task DeleteManyAsync(IEnumerable<TEntity> entities, IDbTransaction transaction = null);
        Task<bool> DeleteManyAsync(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null);
    }
}
