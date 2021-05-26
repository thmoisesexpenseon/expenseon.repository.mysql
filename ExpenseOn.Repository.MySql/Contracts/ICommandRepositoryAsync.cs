namespace ExpenseOn.Repository.MySql
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface ICommandRepositoryAsync<TEntity, TKey> where TEntity : class
    {
        Task<TKey> InsertAsync(TEntity entity);
        Task InsertManyAsync(IEnumerable<TEntity> entities);
        Task<bool> UpdateAsync(TEntity entity);
        Task UpdateManyAsync(IEnumerable<TEntity> entities);
        Task<bool> DeleteAsync(TEntity entity);
        Task<bool> DeleteManyAsync(IEnumerable<TEntity> entities);
        Task<bool> DeleteManyAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
