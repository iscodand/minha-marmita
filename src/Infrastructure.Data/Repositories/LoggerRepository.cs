using Application.Contracts.Repositories;
using Domain.Entities;
using Infrastructure.Data.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class LoggerRepository : GenericRepository<ApplicationLog>, ILoggerRepository
    {
        private readonly DbSet<ApplicationLog> _logs;

        public LoggerRepository(ApplicationDbContext context) : base(context)
        {
            _logs = context.ApplicationLogs;
        }

        public async Task<IEnumerable<ApplicationLog>> GetLogsPagedAsync(int pageNumber, int pageSize)
        {
            return await _logs.AsNoTracking()
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .OrderBy(x => x.TimeStamp)
                            .ToListAsync()
                            .ConfigureAwait(false);
        }
    }
}