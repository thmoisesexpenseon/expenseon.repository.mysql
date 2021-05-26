namespace ExpenseOn.Repository.MySql
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public interface ICommandRepository<TEntity, out TKey> where TEntity : class
    {
        TKey Insert(TEntity entity);
        void InsertMany(IEnumerable<TEntity> entities);
        bool Update(TEntity entity);
        void UpdateMany(IEnumerable<TEntity> entities);
        bool Delete(TEntity entity);
        bool DeleteMany(IEnumerable<TEntity> entities);
        bool DeleteMany(Expression<Func<TEntity, bool>> predicate);
    }
}
