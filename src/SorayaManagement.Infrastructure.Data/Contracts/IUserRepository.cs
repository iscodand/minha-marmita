using SorayaManagement.Domain.Entities;

namespace SorayaManagement.Infrastructure.Data.Contracts
{
    public interface IUserRepository
    {
        public Task<User> GetUserByUsernameAsync(string username);
    }
}