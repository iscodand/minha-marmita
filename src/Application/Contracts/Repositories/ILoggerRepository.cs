using Domain.Entities;

namespace Application.Contracts.Repositories
{
    public interface ILoggerRepository : IGenericRepository<ApplicationLog>
    {
        public Task<IEnumerable<ApplicationLog>> GetLogsPagedAsync(int pageNumber, int pageSize);
    }
}