namespace ExpenseOn.Repository.MySql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Dommel;

    /// <summary>
    ///     Provides a repository pattern implementation for executing commands and queries for entities of type <typeparamref name="TEntity"/> against relational databases.
    /// </summary>
    /// <typeparam name="TEntity">The entity type that the repository operates.</typeparam>
    /// <typeparam name="TKey">The type of the entity's primary key.</typeparam>
    public abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey>, IAsyncRepository<TEntity, TKey>, IDisposable where TEntity : class
    {
        private ColumnPropertyInfo _pkPropertyMap;

        protected IDbConnection DbConnection { get; }

        private bool _isDisposed;

        protected Repository(IDbConnection dbConnection)
        {
            DbConnection = dbConnection;
        }

        public virtual TKey Insert(TEntity entity, IDbTransaction transaction = null)
        {
            return (TKey)Convert.ChangeType(DbConnection.Insert(entity, transaction), typeof(TKey));
        }

        public virtual void InsertMany(IEnumerable<TEntity> entities, IDbTransaction transaction = null)
        {
            DbConnection.InsertAll(entities, transaction);
        }

        public virtual bool Update(TEntity entity, IDbTransaction transaction = null)
        {
            return DbConnection.Update(entity, transaction);
        }

        public virtual void UpdateMany(IEnumerable<TEntity> entities, IDbTransaction transaction = null)
        {
            foreach (var entity in entities)
            {
                Update(entity, transaction);
            }
        }

        public virtual TKey Upsert(TEntity entity, IDbTransaction transaction = null)
        {
            if (IsPrimaryKeyValueSet(entity, out var primaryKeyValue))
            {
                Update(entity, transaction);
                return primaryKeyValue;
            }

            return Insert(entity, transaction);
        }

        public virtual void UpsertMany(IEnumerable<TEntity> entities, IDbTransaction transaction = null)
        {
            if (entities.ToList() is not { Count: > 0 } listedEntities) return;

            if (listedEntities.Where(t => !IsPrimaryKeyValueSet(t, out _)).ToList() is { Count: > 0 } insertList)
                InsertMany(insertList, transaction);

            if (listedEntities.Where(t => IsPrimaryKeyValueSet(t, out _)).ToList() is { Count: > 0 } updateList)
                UpdateMany(updateList, transaction);
        }

        public virtual bool Delete(TEntity entity, IDbTransaction transaction = null)
        {
            return DbConnection.Delete(entity, transaction);
        }

        public virtual void DeleteMany(IEnumerable<TEntity> entities, IDbTransaction transaction = null)
        {
            if (entities.ToList() is not { Count: > 0 } listedEntities) return;

            foreach (var entity in listedEntities)
            {
                Delete(entity, transaction);
            }
        }

        public virtual bool DeleteMany(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null)
        {
            return DbConnection.DeleteMultiple(predicate, transaction) > 0;
        }

        public virtual bool Any(IDbTransaction transaction = null)
        {
            return DbConnection.Any<TEntity>(transaction);
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null)
        {
            return DbConnection.Any(predicate, transaction);
        }

        public virtual long Count(IDbTransaction transaction = null)
        {
            return DbConnection.Count<TEntity>(transaction);
        }

        public virtual long Count(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null)
        {
            return DbConnection.Count(predicate, transaction);
        }

        public virtual TEntity Find(TKey key, IDbTransaction transaction = null)
        {
            return DbConnection.Get<TEntity>(key, transaction);
        }

        public virtual IReadOnlyList<TEntity> GetAll(IDbTransaction transaction = null)
        {
            return DbConnection.GetAll<TEntity>(transaction).ToList();
        }

        public virtual (IReadOnlyList<TEntity> entities, long count) GetAll(int page, int pageSize, IDbTransaction transaction = null)
        {
            var count = DbConnection.Count<TEntity>(transaction);

            return (DbConnection.GetPaged<TEntity>(page, pageSize, transaction).ToList(), count);
        }

        public virtual IReadOnlyList<TEntity> Get(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null)
        {
            return DbConnection.Select(predicate, transaction).ToList();
        }

        public virtual (IReadOnlyList<TEntity> entities, long count) Get(Expression<Func<TEntity, bool>> predicate, int page, int pageSize, IDbTransaction transaction = null)
        {
            var count = DbConnection.Count(predicate, transaction);

            return (DbConnection.SelectPaged(predicate, page, pageSize, transaction).ToList(), count);
        }

        public virtual TEntity FirstOrDefault(IDbTransaction transaction = null)
        {
            return DbConnection.FirstOrDefault<TEntity>(t => t != null, transaction);
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null)
        {
            return DbConnection.FirstOrDefault(predicate, transaction);
        }

        public virtual async Task<TKey> InsertAsync(TEntity entity, IDbTransaction transaction = null)
        {
            return (TKey)Convert.ChangeType(await DbConnection.InsertAsync(entity, transaction), typeof(TKey));
        }

        public virtual Task InsertManyAsync(IEnumerable<TEntity> entities, IDbTransaction transaction = null)
        {
            return DbConnection.InsertAllAsync(entities, transaction);
        }

        public virtual Task<bool> UpdateAsync(TEntity entity, IDbTransaction transaction = null)
        {
            return DbConnection.UpdateAsync(entity, transaction);
        }

        public virtual async Task UpdateManyAsync(IEnumerable<TEntity> entities, IDbTransaction transaction = null)
        {
            foreach (var entity in entities)
            {
                await UpdateAsync(entity, transaction);
            }
        }

        public virtual async Task<TKey> UpsertAsync(TEntity entity, IDbTransaction transaction = null)
        {
            if (IsPrimaryKeyValueSet(entity, out var primaryKeyValue))
            {
                await UpdateAsync(entity, transaction);
                return primaryKeyValue;
            }

            return await InsertAsync(entity, transaction);
        }

        public virtual async Task UpsertManyAsync(IEnumerable<TEntity> entities, IDbTransaction transaction = null)
        {
            if (entities.ToList() is not { Count: > 0 } listedEntities) return;

            if (listedEntities.Where(t => !IsPrimaryKeyValueSet(t, out _)).ToList() is { Count: > 0 } insertList)
                await InsertManyAsync(insertList, transaction);

            if (listedEntities.Where(t => IsPrimaryKeyValueSet(t, out _)).ToList() is { Count: > 0 } updateList)
                await UpdateManyAsync(updateList, transaction);
        }

        public virtual Task<bool> DeleteAsync(TEntity entity, IDbTransaction transaction = null)
        {
            return DbConnection.DeleteAsync(entity, transaction);
        }

        public virtual async Task DeleteManyAsync(IEnumerable<TEntity> entities, IDbTransaction transaction = null)
        {
            if (entities.ToList() is not { Count: > 0 } listedEntities) return;

            foreach (var entity in listedEntities)
            {
                await DeleteAsync(entity, transaction);
            }
        }

        public virtual async Task<bool> DeleteManyAsync(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null)
        {
            return await DbConnection.DeleteMultipleAsync(predicate, transaction) > 0;
        }

        public virtual Task<bool> AnyAsync(IDbTransaction transaction = null)
        {
            return DbConnection.AnyAsync<TEntity>(transaction);
        }

        public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null)
        {
            return DbConnection.AnyAsync(predicate, transaction);
        }

        public virtual Task<long> CountAsync(IDbTransaction transaction = null)
        {
            return DbConnection.CountAsync<TEntity>(transaction);
        }

        public virtual Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null)
        {
            return DbConnection.CountAsync(predicate, transaction);
        }

        public virtual Task<TEntity> FindAsync(TKey key, IDbTransaction transaction = null)
        {
            return DbConnection.GetAsync<TEntity>(key, transaction);
        }

        public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync(IDbTransaction transaction = null)
        {
            return (await DbConnection.GetAllAsync<TEntity>(transaction)).ToList();
        }

        public virtual async Task<(IReadOnlyList<TEntity> entities, long count)> GetAllAsync(int page, int pageSize, IDbTransaction transaction = null)
        {
            var count = await DbConnection.CountAsync<TEntity>(transaction);

            return ((await DbConnection.GetPagedAsync<TEntity>(page, pageSize, transaction)).ToList(), count);
        }

        public virtual async Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null)
        {
            return (await DbConnection.SelectAsync(predicate, transaction)).ToList();
        }

        public virtual async Task<(IReadOnlyList<TEntity> entities, long count)> GetAsync(Expression<Func<TEntity, bool>> predicate, int page, int pageSize, IDbTransaction transaction = null)
        {
            var count = await DbConnection.CountAsync(predicate, transaction);

            return ((await DbConnection.SelectPagedAsync(predicate, page, pageSize, transaction)).ToList(), count);
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(IDbTransaction transaction = null)
        {
            return DbConnection.FirstOrDefaultAsync<TEntity>(t => t != null, transaction);
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null)
        {
            return DbConnection.FirstOrDefaultAsync(predicate, transaction);
        }

        protected bool IsPrimaryKeyValueSet(TEntity entity, out TKey primaryKey)
        {
            primaryKey = GetPrimaryKeyValue(entity);
            return !EqualityComparer<TKey>.Default.Equals(primaryKey, default);
        }

        protected TKey GetPrimaryKeyValue(TEntity entity)
        {
            if(_pkPropertyMap is null)
            {
                _pkPropertyMap = Resolvers.KeyProperties(typeof(TEntity)).First();
            }
            return (TKey)Convert.ChangeType(_pkPropertyMap.Property.GetValue(entity), typeof(TKey));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                DbConnection.Close();
                DbConnection.Dispose();
            }

            _isDisposed = true;
        }
    }
}
