using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Data.Contracts;
using Infrastructure.Data.DataContext;

namespace Infrastructure.Data.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly DbSet<Customer> _customers;

        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
            _customers = context.Customers;
        }

        public async Task<Customer> DetailCustomerAsync(int customerId)
        {
            return await _customers.AsNoTracking()
                                   .Include(x => x.User).AsNoTracking()
                                   .Include(x => x.Company).AsNoTracking()
                                   .Include(x => x.Orders).AsNoTracking()
                                   .Where(x => x.Id == customerId).AsNoTracking()
                                   .FirstOrDefaultAsync()
                                   .ConfigureAwait(false);
        }

        public async Task<ICollection<Customer>> GetCustomersByCompanyAsync(int companyId)
        {
            return await _customers.AsNoTracking()
                                   .Include(x => x.User).AsNoTracking()
                                   .Include(x => x.Company).AsNoTracking()
                                   .Where(x => x.CompanyId == companyId).AsNoTracking()
                                   .ToListAsync()
                                   .ConfigureAwait(false);
        }
    }
}