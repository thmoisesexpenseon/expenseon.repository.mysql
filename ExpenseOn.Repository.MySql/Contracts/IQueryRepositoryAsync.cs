namespace ExpenseOn.Repository.MySql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IQueryRepositoryAsync<TEntity, in TKey> where TEntity : class
    {
        Task<bool> AnyAsync(IDbTransaction transaction = null);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null);
        Task<long> CountAsync(IDbTransaction transaction = null);
        Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null);
        Task<TEntity> FindAsync(TKey key, IDbTransaction transaction = null);
        Task<TEntity> FirstOrDefaultAsync(IDbTransaction transaction = null);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null);
        Task<IReadOnlyList<TEntity>> GetAllAsync(IDbTransaction transaction = null);
        Task<(IReadOnlyList<TEntity> entities, long count)> GetAllAsync(int skip, int take, IDbTransaction transaction = null);
        Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null);
        Task<(IReadOnlyList<TEntity> entities, long count)> GetAsync(Expression<Func<TEntity, bool>> predicate, int skip, int take, IDbTransaction transaction = null);
    }
}