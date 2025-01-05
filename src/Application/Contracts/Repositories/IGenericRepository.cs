using Domain.Entities.Common;

namespace Application.Contracts.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<ICollection<T>> GetAllAsync();
        public Task<int> CountAsync();
        public Task<T> GetByIdAsync(int id);
        public Task<IReadOnlyList<T>> GetPagedResponseAsync(int pageNumber, int pageSize);
        public Task<T> CreateAsync(T entity);
        public Task UpdateAsync(T entity);
        public Task DeleteAsync(T entity);
        public Task SaveAsync();
    }
}