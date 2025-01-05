using Domain.Entities;

namespace Application.Contracts.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        public Task<Order> GetOrderDetailsAsync(int orderId);
        public Task<ICollection<Order>> GetOrdersByCompanyAsync(int companyId);
        public Task<ICollection<Order>> GetOrdersByDateAsync(int companyId, DateTime? date);
        public Task<(ICollection<Order> orders, int count)> GetOrdersByDateRangePagedAsync(int companyId, DateTime initialDate, DateTime endDate, int pageSize, int pageNumber);
        public Task<ICollection<Order>> GetOrdersByDateRangeAsync(int companyId, DateTime? initialDate, DateTime? finalDate);
    }
}