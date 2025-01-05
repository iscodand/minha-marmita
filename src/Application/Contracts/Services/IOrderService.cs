using Application.Dtos.Order;
using Application.Parameters;
using Application.Wrappers;

namespace Application.Contracts.Services
{
        public interface IOrderService
        {
                public Task<Response<IEnumerable<GetOrderDto>>> GetOrdersByCompanyAsync(int companyId);
                public Task<PagedResponse<IEnumerable<GetOrderDto>>> GetOrdersByDateAsync(int companyId, DateTime? date);
                public Task<PagedResponse<IEnumerable<GetOrderDto>>> GetOrdersByDateRangePagedAsync(int companyId, RequestParameter parameter);
                public Task<Response<IEnumerable<GetOrderDto>>> GetOrdersByDateRangeAsync(int companyId, DateTime? initialDate, DateTime? finalDate);
                public Task<Response<DetailOrderDto>> GetOrderDetailsAsync(int orderId, int userCompanyId);
                public Task<Response<CreateOrderDto>> CreateOrderAsync(CreateOrderDto createOrderDto);
                public Task<Response<GetCreateOrderItemsDto>> GetCreateOrdersItemsAsync(int companyId);
                public Task<Response<UpdateOrderDto>> UpdateOrderAsync(UpdateOrderDto updateOrderDto);
                public Task<Response<UpdateOrderDto>> MakeOrderPaymentAsync(int orderId, int userCompanyId);
                public Task<Response<GetOrderDto>> DeleteOrderAsync(int orderId, int userCompanyId);
        }
}