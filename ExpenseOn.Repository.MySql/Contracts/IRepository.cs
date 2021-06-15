namespace ExpenseOn.Repository.MySql
{
    /// <summary>
    ///     Provides methods for executing commands and queries against a relational database.
    /// </summary>
    /// <remarks>
    ///     This is a convenience interface that inherits <see cref="ICommandRepository{TEntity,TKey}"/> and <see cref="IQueryRepository{TEntity,TKey}"/> interfaces only.
    /// </remarks>
    /// <typeparam name="TEntity">The entity type that the repository operates.</typeparam>
    /// <typeparam name="TKey">The type of the entity's primary key.</typeparam>
    public interface IRepository<TEntity, TKey> : ICommandRepository<TEntity, TKey>, IQueryRepository<TEntity, TKey> where TEntity : class
    {
    }
}