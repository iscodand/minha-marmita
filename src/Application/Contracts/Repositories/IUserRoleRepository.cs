using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Contracts.Repositories
{
    public interface IUserRoleRepository
    {
        public Task<UserRole> GetUserRoleAsync(string userId);
    }
}