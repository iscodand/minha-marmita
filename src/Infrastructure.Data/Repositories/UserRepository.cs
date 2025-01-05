using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Contracts.Repositories;
using Infrastructure.Data.DataContext;

namespace Infrastructure.Data.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly DbSet<User> _users;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _users = context.Set<User>();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _users.AsNoTracking()
                               .Include(x => x.UserCompany)
                               .Where(x => x.NormalizedEmail == email.ToUpper().Trim())
                               .FirstOrDefaultAsync()
                               .ConfigureAwait(false);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _users.AsNoTracking()
                               .Include(x => x.UserCompany)
                               .FirstOrDefaultAsync(x => x.NormalizedUserName == username.Trim().ToUpper())
                               .ConfigureAwait(false);
        }

        public async Task<ICollection<User>> GetUsersByCompanyAsync(int companyId)
        {
            return await _users.AsNoTracking()
                               .Where(x => x.CompanyId == companyId)
                               .ToListAsync()
                               .ConfigureAwait(false);
        }

        public async Task<(IEnumerable<User> users, int count)> GetUsersByCompanyPagedAsync(int companyId, int pageNumber, int pageSize)
        {
            IEnumerable<User> users = await _users.AsNoTracking()
                            .Where(x => x.CompanyId == companyId)
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync()
                            .ConfigureAwait(false);

            int totalItems = await _users.AsNoTracking()
                                .Where(x => x.CompanyId == companyId)
                                .CountAsync();

            return (users, totalItems);
        }

        public async Task<User> GetWithOrdersAsync(string username)
        {
            return await _users.AsNoTracking()
                   .Include(x => x.UserCompany)
                   .Include(x => x.Orders).ThenInclude(x => x.PaymentType)
                   .Include(x => x.Orders).ThenInclude(x => x.Customer)
                   .Include(x => x.Orders).ThenInclude(x => x.Meal)
                   .FirstOrDefaultAsync(x => x.NormalizedUserName == username.Trim().ToUpper())
                   .ConfigureAwait(false);
        }

        public async Task<bool> UserExistsByEmailAsync(string email, string userId = "")
        {
            return await _users.AsNoTracking()
                               .Where(x => x.NormalizedEmail == email.Trim().ToUpper())
                               .Where(x => x.Id != userId)
                               .AnyAsync()
                               .ConfigureAwait(false);
        }

        public async Task<bool> UserExistsByUsernameAsync(string username, string userId = "")
        {
            return await _users.AsNoTracking()
                               .Where(x => x.NormalizedUserName == username.Trim().ToUpper())
                               .Where(x => x.Id != userId)
                               .AnyAsync()
                               .ConfigureAwait(false);
        }
    }
}