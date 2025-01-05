using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.DataContext;
using Application.Contracts.Repositories;
using Domain.Entities;
using Application.Services;

namespace Infrastructure.Data.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly DbSet<Order> _orders;

        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _orders = context.Orders;
        }

        public async Task<Order> GetOrderDetailsAsync(int orderId)
        {
            return await _orders
                   .Include(x => x.User).AsNoTracking()
                   .Include(x => x.Meal).AsNoTracking()
                   .Include(x => x.Customer).AsNoTracking()
                   .Include(x => x.PaymentType).AsNoTracking()
                   .Where(x => x.Id == orderId).AsNoTracking()
                   .FirstOrDefaultAsync()
                   .ConfigureAwait(false);
        }

        public async Task<ICollection<Order>> GetOrdersByCompanyAsync(int companyId)
        {
            return await _orders.AsNoTracking()
                                .Include(x => x.User).AsNoTracking()
                                .Include(x => x.Company).AsNoTracking()
                                .Include(x => x.Meal).AsNoTracking()
                                .Include(x => x.Customer).AsNoTracking()
                                .Include(x => x.PaymentType).AsNoTracking()
                                .Where(x => x.CompanyId == companyId).AsNoTracking()
                                .ToListAsync()
                                .ConfigureAwait(false);
        }

        public async Task<ICollection<Order>> GetOrdersByDateAsync(int companyId, DateTime? date)
        {
            return await _orders.AsNoTracking()
                                .Include(x => x.User)
                                .Include(x => x.Company)
                                .Include(x => x.Meal)
                                .Include(x => x.Customer)
                                .Include(x => x.PaymentType)
                                .Where(x => x.CreatedAt.Date == date.Value.Date)
                                .Where(x => x.CompanyId == companyId)
                                .ToListAsync()
                                .ConfigureAwait(false);
        }

        public async Task<(ICollection<Order> orders, int count)> GetOrdersByDateRangePagedAsync(int companyId, DateTime initialDate, DateTime endDate, int pageSize, int pageNumber)
        {
            var orders = await _orders.AsNoTracking()
                                .Include(x => x.User)
                                .Include(x => x.Company)
                                .Include(x => x.Meal)
                                .Include(x => x.Customer)
                                .Include(x => x.PaymentType)
                                    .Where(x => x.CreatedAt.Date >= initialDate.Date &&
                                                x.CreatedAt.Date <= endDate.Date)
                                    .Where(x => x.CompanyId == companyId)
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync()
                                .ConfigureAwait(false);

            int totalCount = await _orders.AsNoTracking()
                                    .Where(x => x.CreatedAt.Date >= initialDate.Date &&
                                                x.CreatedAt.Date <= endDate.Date)
                                    .Where(x => x.CompanyId == companyId)
                                    .CountAsync()
                                    .ConfigureAwait(false);

            return (orders, totalCount);
        }

        public async Task<ICollection<Order>> GetOrdersByDateRangeAsync(int companyId, DateTime? initialDate, DateTime? finalDate)
        {
            return await _orders.AsNoTracking()
                                .Include(x => x.Customer).AsNoTracking()
                                .Include(x => x.Meal).AsNoTracking()
                                .Include(x => x.PaymentType).AsNoTracking()
                                .Where(x => x.CreatedAt.Date >= initialDate.Value.Date && x.CreatedAt.Date <= finalDate.Value.Date).AsNoTracking()
                                .Where(x => x.CompanyId == companyId).AsNoTracking()
                                .ToListAsync()
                                .ConfigureAwait(false);
        }
    }
}