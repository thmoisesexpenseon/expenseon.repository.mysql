namespace ExpenseOn.Repository.MySql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq.Expressions;

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
