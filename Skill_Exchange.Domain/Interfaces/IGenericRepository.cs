namespace Skill_Exchange.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        // Add a new entity
        Task<bool> AddAsync(T entity);
        // Update an existing entity
        Task<bool> UpdateAsync(T entity);
        // Delete an entity
        Task<bool> DeleteAsync(T entity);
        // Get an entity by its ID
        Task<T> GetByIdAsync(Guid id);
        // Get all entities
        Task<IEnumerable<T>> GetAllAsync();
    }
}
