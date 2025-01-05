using Application.Contracts.Repositories;
using Domain.Entities;
using Infrastructure.Data.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly DbSet<Customer> _customers;

        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
            _customers = context.Customers;
        }
        public async Task<Customer> GetCustomerByIdAsync(int customerId)
        {
            return await _customers.AsNoTracking()
                                   .Include(x => x.User).AsNoTracking()
                                   .Where(x => x.Id == customerId)
                                   .FirstOrDefaultAsync()
                                   .ConfigureAwait(false);
        }

        public async Task<Customer> DetailCustomerAsync(int customerId)
        {
            return await _customers.AsNoTracking()
                                   .Include(x => x.User).AsNoTracking()
                                   .Include(x => x.Company).AsNoTracking()
                                   .Include(x => x.Orders)
                                   .ThenInclude(x => x.Meal).AsNoTracking()
                                   .Include(x => x.Orders.Skip((1 - 1) * 10).Take(10))
                                   .ThenInclude(x => x.PaymentType).AsNoTracking()
                                   .Where(x => x.Id == customerId).AsNoTracking()
                                   .FirstOrDefaultAsync()
                                   .ConfigureAwait(false);
        }

        public async Task<ICollection<Customer>> GetActiveCustomersByCompanyAsync(int companyId)
        {
            return await _customers.AsNoTracking()
                                   .Include(x => x.User).AsNoTracking()
                                   .Include(x => x.Company).AsNoTracking()
                                   .Where(x => x.CompanyId == companyId).AsNoTracking()
                                   .Where(x => x.IsActive == true).AsNoTracking()
                                   .ToListAsync()
                                   .ConfigureAwait(false);
        }

        public async Task<ICollection<Customer>> GetInactiveCustomersByCompanyAsync(int companyId)
        {
            return await _customers.AsNoTracking()
                                   .Include(x => x.User).AsNoTracking()
                                   .Include(x => x.Company).AsNoTracking()
                                   .Where(x => x.CompanyId == companyId).AsNoTracking()
                                   .Where(x => x.IsActive == false).AsNoTracking()
                                   .ToListAsync()
                                   .ConfigureAwait(false);
        }

        public async Task<bool> CustomerExistsByCompanyAsync(string name, int companyId)
        {
            return await _customers.Where(x => x.CompanyId == companyId)
                                   .AnyAsync(x => x.Name == name)
                                   .ConfigureAwait(false);
        }

        public async Task<ICollection<Customer>> GetCustomersByDateRangeAsync(int companyId, DateTime initialDate, DateTime finalDate)
        {
            return await _customers.AsNoTracking()
                                   .Include(x => x.Orders.Where(x => x.CreatedAt.Date >= initialDate.Date && x.CreatedAt.Date <= finalDate.Date))
                                   .Where(x => x.CompanyId == companyId)
                                   .ToListAsync()
                                   .ConfigureAwait(false);
        }

        public async Task<(ICollection<Customer> customers, int count)> GetByCompanyPagedAsync(int companyId, int pageNumber, int pageSize)
        {
            var customers = await _customers.AsNoTracking()
                                .Include(x => x.User)
                                .Include(x => x.Company)
                                .Include(x => x.Orders)
                                .Where(x => x.CompanyId == companyId)
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync()
                                .ConfigureAwait(false);

            int totalCustomers = await _customers.AsNoTracking()
                                                .Where(x => x.CompanyId == companyId)
                                                .CountAsync()
                                                .ConfigureAwait(false);

            return (customers, totalCustomers);
        }

        public async Task<ICollection<Customer>> SearchByCustomerAsync(string name)
        {
            return await _customers.AsNoTracking()
                                   .Include(x => x.User)
                                   .Include(x => x.Orders)
                                   .Where(x => x.Name.ToUpper().Trim().Contains(name.ToUpper().Trim()))
                                   .ToListAsync()
                                   .ConfigureAwait(false);
        }
    }
}