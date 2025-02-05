using Application.Contracts.Repositories;
using Domain.Entities;
using Infrastructure.Data.DataContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly DbSet<UserRole> _userRoles;

        public UserRoleRepository(ApplicationDbContext context)
        {
            _userRoles = context.UserRoles;
        }

        public async Task<UserRole> GetUserRoleAsync(string userId)
        {
            return await _userRoles.AsNoTracking()
                                   .Where(x => x.UserId == userId)
                                   .FirstOrDefaultAsync()
                                   .ConfigureAwait(false);
        }
    }
}