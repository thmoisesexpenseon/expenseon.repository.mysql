namespace ExpenseOn.Repository.MySql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

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
