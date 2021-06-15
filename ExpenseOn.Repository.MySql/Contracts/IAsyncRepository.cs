namespace ExpenseOn.Repository.MySql
{
    /// <summary>
    ///     Provides asynchronous methods for executing commands and queries against a relational database.
    /// </summary>
    /// <remarks>
    ///     This is a convenience interface that inherits <see cref="ICommandRepositoryAsync{TEntity,TKey}"/> and <see cref="IQueryRepositoryAsync{TEntity,TKey}"/> interfaces only.
    /// </remarks>
    /// <typeparam name="TEntity">The entity type that the repository operates.</typeparam>
    /// <typeparam name="TKey">The type of the entity's primary key.</typeparam>
    public interface IAsyncRepository<TEntity, TKey> : ICommandRepositoryAsync<TEntity, TKey>, IQueryRepositoryAsync<TEntity, TKey> where TEntity : class
    {
    }
}