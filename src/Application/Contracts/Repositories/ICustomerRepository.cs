using Domain.Entities;

namespace Application.Contracts.Repositories
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        public Task<(ICollection<Customer> customers, int count)> GetByCompanyPagedAsync(int companyId, int pageNumber, int pageSize);
        public Task<ICollection<Customer>> GetCustomersByDateRangeAsync(int companyId, DateTime initialDate, DateTime finalDate);
        public Task<ICollection<Customer>> GetActiveCustomersByCompanyAsync(int companyId);
        public Task<ICollection<Customer>> GetInactiveCustomersByCompanyAsync(int companyId);
        public Task<ICollection<Customer>> SearchByCustomerAsync(string name);
        public Task<Customer> DetailCustomerAsync(int customerId);
        public Task<Customer> GetCustomerByIdAsync(int customerId);
        public Task<bool> CustomerExistsByCompanyAsync(string name, int companyId);
    }
}