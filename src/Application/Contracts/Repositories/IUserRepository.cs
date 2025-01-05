using Domain.Entities;

namespace Application.Contracts.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public Task<User> GetUserByUsernameAsync(string username);
        public Task<User> GetUserByEmailAsync(string email);
        public Task<User> GetWithOrdersAsync(string username);
        public Task<ICollection<User>> GetUsersByCompanyAsync(int companyId);
        public Task<(IEnumerable<User> users, int count)> GetUsersByCompanyPagedAsync(int companyId, int pageNumber, int pageSize);
        public Task<bool> UserExistsByUsernameAsync(string username, string userId = "");
        public Task<bool> UserExistsByEmailAsync(string email, string userId = "");
    }
}