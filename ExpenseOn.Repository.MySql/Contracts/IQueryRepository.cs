namespace ExpenseOn.Repository.MySql
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public interface IQueryRepository<TEntity, in TKey> where TEntity : class
    {
        bool Any();
        bool Any(Expression<Func<TEntity, bool>> predicate);
        long Count();
        long Count(Expression<Func<TEntity, bool>> predicate);
        TEntity Find(TKey key);
        TEntity FirstOrDefault();
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        IReadOnlyList<TEntity> GetAll();
        (IReadOnlyList<TEntity> entities, long count) GetAll(int skip, int take);
        IReadOnlyList<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        (IReadOnlyList<TEntity> entities, long count) Get(Expression<Func<TEntity, bool>> predicate, int skip, int take);
    }
}
