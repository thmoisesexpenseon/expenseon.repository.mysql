namespace ExpenseOn.Repository.MySql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Dommel;

    public abstract class Repository<TEntity, TKey> : ICommandRepository<TEntity, TKey>, ICommandRepositoryAsync<TEntity, TKey>, IQueryRepository<TEntity, TKey>, IQueryRepositoryAsync<TEntity, TKey> where TEntity : class
    {
        protected IDbConnection DbConnection { get; }

        protected Repository(IDbConnection dbConnection)
        {
            DbConnection = dbConnection;
        }

        public virtual TKey Insert(TEntity entity)
        {
            return (TKey)Convert.ChangeType(DbConnection.Insert(entity), typeof(TKey));
        }

        public virtual void InsertMany(IEnumerable<TEntity> entities)
        {
            DbConnection.InsertAll(entities);
        }

        public virtual bool Update(TEntity entity)
        {
            return DbConnection.Update(entity);
        }

        public virtual void UpdateMany(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                DbConnection.Update(entity);
            }
        }

        public virtual bool Delete(TEntity entity)
        {
            return DbConnection.Delete(entity);
        }

        public virtual bool DeleteMany(IEnumerable<TEntity> entities)
        {
            var listedEntities = entities.ToList();

            if (!listedEntities.Any())
                return false;

            foreach (var entity in listedEntities)
            {
                DbConnection.Delete(entity);
            }

            return true;
        }

        public virtual bool DeleteMany(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = DbConnection.Select(predicate).ToList();

            return DeleteMany(entities);
        }

        public virtual bool Any()
        {
            return DbConnection.Any<TEntity>();
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return DbConnection.Any(predicate);
        }

        public virtual long Count()
        {
            return DbConnection.Count<TEntity>();
        }

        public virtual long Count(Expression<Func<TEntity, bool>> predicate)
        {
            return DbConnection.Count(predicate);
        }

        public virtual TEntity Find(TKey key)
        {
            return DbConnection.Get<TEntity>(key);
        }

        public virtual IReadOnlyList<TEntity> GetAll()
        {
            return DbConnection.GetAll<TEntity>().ToList();
        }

        public virtual (IReadOnlyList<TEntity> entities, long count) GetAll(int skip, int take)
        {
            var count = DbConnection.Count<TEntity>();

            return (DbConnection.GetPaged<TEntity>(skip, take).ToList(), count);
        }

        public virtual IReadOnlyList<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return DbConnection.Select(predicate).ToList();
        }

        public virtual (IReadOnlyList<TEntity> entities, long count) Get(Expression<Func<TEntity, bool>> predicate, int skip, int take)
        {
            var count = DbConnection.Count(predicate);

            return (DbConnection.SelectPaged(predicate, skip, take).ToList(), count);
        }

        public virtual TEntity FirstOrDefault()
        {
            return DbConnection.FirstOrDefault<TEntity>(t => t != null);
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return DbConnection.FirstOrDefault(predicate);
        }

        public virtual async Task<TKey> InsertAsync(TEntity entity)
        {
            return (TKey)Convert.ChangeType(await DbConnection.InsertAsync(entity), typeof(TKey));
        }

        public virtual Task InsertManyAsync(IEnumerable<TEntity> entities)
        {
            return DbConnection.InsertAllAsync(entities);
        }

        public virtual Task<bool> UpdateAsync(TEntity entity)
        {
            return DbConnection.UpdateAsync(entity);
        }

        public virtual async Task UpdateManyAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                await DbConnection.UpdateAsync(entity);
            }
        }

        public virtual Task<bool> DeleteAsync(TEntity entity)
        {
            return DbConnection.DeleteAsync(entity);
        }

        public virtual async Task<bool> DeleteManyAsync(IEnumerable<TEntity> entities)
        {
            var listedEntities = entities.ToList();

            if (!listedEntities.Any())
                return false;

            foreach (var entity in listedEntities)
            {
                await DbConnection.DeleteAsync(entity);
            }

            return true;
        }

        public virtual async Task<bool> DeleteManyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = (await DbConnection.SelectAsync(predicate)).ToList();

            return DeleteMany(entities);
        }

        public virtual Task<bool> AnyAsync()
        {
            return DbConnection.AnyAsync<TEntity>();
        }

        public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return DbConnection.AnyAsync(predicate);
        }

        public virtual Task<long> CountAsync()
        {
            return DbConnection.CountAsync<TEntity>();
        }

        public virtual Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return DbConnection.CountAsync(predicate);
        }

        public virtual Task<TEntity> FindAsync(TKey key)
        {
            return DbConnection.GetAsync<TEntity>(key);
        }

        public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync()
        {
            return (await DbConnection.GetAllAsync<TEntity>()).ToList();
        }

        public virtual async Task<(IReadOnlyList<TEntity> entities, long count)> GetAllAsync(int skip, int take)
        {
            var count = await DbConnection.CountAsync<TEntity>();

            return ((await DbConnection.GetAllAsync<TEntity>()).ToList(), count);
        }

        public virtual async Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return (await DbConnection.SelectAsync(predicate)).ToList();
        }

        public virtual async Task<(IReadOnlyList<TEntity> entities, long count)> GetAsync(Expression<Func<TEntity, bool>> predicate, int skip, int take)
        {
            var count = await DbConnection.CountAsync<TEntity>();

            return ((await DbConnection.SelectPagedAsync(predicate, skip, take)).ToList(), count);
        }

        public virtual Task<TEntity> FirstOrDefaultAsync()
        {
            return DbConnection.FirstOrDefaultAsync<TEntity>(t => t != null);
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return DbConnection.FirstOrDefaultAsync(predicate);
        }
    }
}
