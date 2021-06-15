namespace ExpenseOn.Repository.MySql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Dapper.FluentMap;
    using Dapper.FluentMap.Dommel.Mapping;
    using Dommel;

    public abstract class Repository<TEntity, TKey> : ICommandRepository<TEntity, TKey>, ICommandRepositoryAsync<TEntity, TKey>, IQueryRepository<TEntity, TKey>, IQueryRepositoryAsync<TEntity, TKey> where TEntity : class
    {
        private readonly DommelPropertyMap _pkPropertyMap;

        protected IDbConnection DbConnection { get; }

        protected Repository(IDbConnection dbConnection)
        {
            DbConnection = dbConnection;

            if (FluentMapper.EntityMaps.TryGetValue(typeof(TEntity), out var entityMap))
            {
                _pkPropertyMap = entityMap.PropertyMaps.OfType<DommelPropertyMap>().SingleOrDefault(t => t.Key);
            }
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
            if (GetPrimaryKeyValue(entity) is { } primaryKeyValue)
            {
                Update(entity, transaction);
                return primaryKeyValue;
            }

            return Insert(entity, transaction);
        }

        public virtual void UpsertMany(IEnumerable<TEntity> entities, IDbTransaction transaction = null)
        {
            if (entities.ToList() is not { Count: > 0 } listedEntities) return;

            if (listedEntities.Where(t => !IsPrimaryKeyValueSet(t)).ToList() is { Count: > 0 } insertList)
                InsertMany(insertList, transaction);

            if (listedEntities.Where(IsPrimaryKeyValueSet).ToList() is { Count: > 0 } updateList)
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
            return DbConnection.DeleteMultiple(predicate) > 0;
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

        public virtual (IReadOnlyList<TEntity> entities, long count) GetAll(int skip, int take, IDbTransaction transaction = null)
        {
            var count = DbConnection.Count<TEntity>(transaction);

            return (DbConnection.GetPaged<TEntity>(skip, take, transaction).ToList(), count);
        }

        public virtual IReadOnlyList<TEntity> Get(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null)
        {
            return DbConnection.Select(predicate, transaction).ToList();
        }

        public virtual (IReadOnlyList<TEntity> entities, long count) Get(Expression<Func<TEntity, bool>> predicate, int skip, int take, IDbTransaction transaction = null)
        {
            var count = DbConnection.Count(predicate, transaction);

            return (DbConnection.SelectPaged(predicate, skip, take, transaction).ToList(), count);
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
            if (GetPrimaryKeyValue(entity) is { } primaryKeyValue)
            {
                await UpdateAsync(entity, transaction);
                return primaryKeyValue;
            }

            return await InsertAsync(entity, transaction);
        }

        public virtual async Task UpsertManyAsync(IEnumerable<TEntity> entities, IDbTransaction transaction = null)
        {
            if (entities.ToList() is not { Count: > 0 } listedEntities) return;

            if (listedEntities.Where(t => !IsPrimaryKeyValueSet(t)).ToList() is { Count: > 0 } insertList)
                await InsertManyAsync(insertList, transaction);

            if (listedEntities.Where(IsPrimaryKeyValueSet).ToList() is { Count: > 0 } updateList)
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

        public virtual async Task<(IReadOnlyList<TEntity> entities, long count)> GetAllAsync(int skip, int take, IDbTransaction transaction = null)
        {
            var count = await DbConnection.CountAsync<TEntity>(transaction);

            return ((await DbConnection.GetAllAsync<TEntity>(transaction)).ToList(), count);
        }

        public virtual async Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null)
        {
            return (await DbConnection.SelectAsync(predicate, transaction)).ToList();
        }

        public virtual async Task<(IReadOnlyList<TEntity> entities, long count)> GetAsync(Expression<Func<TEntity, bool>> predicate, int skip, int take, IDbTransaction transaction = null)
        {
            var count = await DbConnection.CountAsync(predicate, transaction);

            return ((await DbConnection.SelectPagedAsync(predicate, skip, take, transaction)).ToList(), count);
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(IDbTransaction transaction = null)
        {
            return DbConnection.FirstOrDefaultAsync<TEntity>(t => t != null, transaction);
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null)
        {
            return DbConnection.FirstOrDefaultAsync(predicate, transaction);
        }

        protected bool IsPrimaryKeyValueSet(TEntity entity) => !EqualityComparer<TKey>.Default.Equals(GetPrimaryKeyValue(entity), default);

        protected TKey GetPrimaryKeyValue(TEntity entity)
        {
            if (_pkPropertyMap == null)
                throw new InvalidOperationException($"No primary key is defined for type '{typeof(TEntity).Name}'. The current operation requires a primary key to be defined using Dommel's fluent mapper.");

            return (TKey)Convert.ChangeType(_pkPropertyMap.PropertyInfo.GetValue(entity), typeof(TKey));
        }
    }
}
