namespace FaceLock.Domain.Repositories
{
    /// <summary>
    /// Generic interface representing CRUD methods to database 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// A method that returns a entity from the database by entity id
        /// </summary>
        /// <param name="id">Entity id</param>
        /// <returns>TEntity where TEntity is an entity from the database</returns>
        Task<TEntity> GetByIdAsync(int id);
        /// <summary>
        /// A method that returns a list of entities from the database
        /// </summary>
        /// <returns>List<TEntity> where List<TEntity> is a list of entities from the database</returns>
        Task<List<TEntity>> GetAllAsync();
        /// <summary>
        /// A method that adds an entity to databases
        /// </summary>
        /// <param name="entity">An entity that is added to the database</param>
        /// <returns></returns>
        Task AddAsync(TEntity entity);
        /// <summary>
        /// A method that updates an entity in the database
        /// </summary>
        /// <param name="entity">An entity that is updated in the database</param>
        /// <returns></returns>
        Task UpdateAsync(TEntity entity);
        /// <summary>
        /// A method that deletes an entity from the database
        /// </summary>
        /// <param name="entity">The entity to be deleted from the database</param>
        /// <returns></returns>
        Task DeleteAsync(TEntity entity);
    }
}
