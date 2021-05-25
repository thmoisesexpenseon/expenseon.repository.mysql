namespace ExpenseOn.Repository.MySql
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IRepository<TEntity, TKey> where TEntity : class
    {
        TKey Insert(TEntity entity);
        void InsertMany(IEnumerable<TEntity> entities);
        bool Update(TEntity entity);
        bool Delete(TEntity entity);
        bool DeleteMany(IEnumerable<TEntity> entities);
        bool DeleteMany(Expression<Func<TEntity, bool>> predicate);
        bool Any();
        bool Any(Expression<Func<TEntity, bool>> predicate);
        long Count();
        long Count(Expression<Func<TEntity, bool>> predicate);
        TEntity Find(TKey key);
        IReadOnlyList<TEntity> GetAll();
        (IReadOnlyList<TEntity> entities, long count) GetAll(int skip, int take);
        IReadOnlyList<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        (IReadOnlyList<TEntity> entities, long count) Get(Expression<Func<TEntity, bool>> predicate, int skip, int take);
        TEntity FirstOrDefault();
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        Task<TKey> InsertAsync(TEntity entity);
        Task InsertManyAsync(IEnumerable<TEntity> entities);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(TEntity entity);
        Task<bool> DeleteManyAsync(IEnumerable<TEntity> entities);
        Task<bool> DeleteManyAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyAsync();
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        Task<long> CountAsync();
        Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FindAsync(TKey key);
        Task<IReadOnlyList<TEntity>> GetAllAsync();
        Task<(IReadOnlyList<TEntity> entities, long count)> GetAllAsync(int skip, int take);
        Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);
        Task<(IReadOnlyList<TEntity> entities, long count)> GetAsync(Expression<Func<TEntity, bool>> predicate, int skip, int take);
        Task<TEntity> FirstOrDefaultAsync();
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
