using Application.Dtos.Order;
using Application.Wrappers;
using Domain.Entities;
using Application.Contracts.Repositories;
using Infrastructure.Data.Repositories;
using Application.Contracts.Services;
using Application.Parameters;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentTypeRepository _paymentTypeRepository;
        private readonly IMealRepository _mealRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICompanyRepository _companyRepository;

        public OrderService(IOrderRepository orderRepository,
                            IPaymentTypeRepository paymentTypeRepository,
                            IMealRepository mealRepository,
                            ICustomerRepository customerRepository,
                            ICompanyRepository companyRepository)
        {
            _orderRepository = orderRepository;
            _paymentTypeRepository = paymentTypeRepository;
            _mealRepository = mealRepository;
            _customerRepository = customerRepository;
            _companyRepository = companyRepository;
        }

        public async Task<Response<CreateOrderDto>> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            if (createOrderDto == null)
            {
                return new Response<CreateOrderDto>()
                {
                    Message = "O Pedido não pode ser nulo.",
                    Succeeded = false,
                };
            }

            Order order = Order.Create(
                createOrderDto.Description,
                createOrderDto.Price,
                createOrderDto.PaymentTypeId,
                createOrderDto.MealId,
                createOrderDto.CustomerId,
                createOrderDto.CompanyId,
                createOrderDto.UserId
            );

            await _orderRepository.CreateAsync(order);

            return new Response<CreateOrderDto>()
            {
                Message = "Pedido criado com sucesso.",
                Succeeded = true
            };
        }

        public async Task<Response<UpdateOrderDto>> UpdateOrderAsync(UpdateOrderDto updateOrderDto)
        {
            if (updateOrderDto == null)
            {
                return new Response<UpdateOrderDto>()
                {
                    Message = "Pedido não pode ser nulo.",
                    Succeeded = false
                };
            }

            Order order = await _orderRepository.GetByIdAsync(updateOrderDto.OrderId);

            if (order.CompanyId != updateOrderDto.CompanyId)
            {
                return new Response<UpdateOrderDto>()
                {
                    Message = "Você não pode editar esse pedido. Ele não pertence a sua empresa.",
                    Succeeded = false
                };
            }

            order.Description = updateOrderDto.Description;
            order.CustomerId = updateOrderDto.CustomerId;
            order.MealId = updateOrderDto.MealId;
            order.PaymentTypeId = updateOrderDto.PaymentTypeId;
            order.Price = updateOrderDto.Price;
            order.UpdatedAt = DateTime.Now;

            // Validate order payment
            if (order.IsPaid == true && updateOrderDto.IsPaid == false)
            {
                order.IsPaid = false;
                order.PaidAt = null;
            }
            else if (order.IsPaid == false && updateOrderDto.IsPaid == true)
            {
                order.IsPaid = true;
                order.PaidAt = DateTime.Now;
            }

            await _orderRepository.UpdateAsync(order);

            return new Response<UpdateOrderDto>()
            {
                Message = $"Pedido N° {updateOrderDto.OrderId} atualizado com sucesso.",
                Succeeded = true
            };
        }

        public async Task<Response<DetailOrderDto>> GetOrderDetailsAsync(int orderId, int userCompanyId)
        {
            Order order = await _orderRepository.GetOrderDetailsAsync(orderId);

            if (order == null)
            {
                return new Response<DetailOrderDto>()
                {
                    Message = "O pedido não foi encontrado.",
                    Succeeded = false,
                };
            }

            if (order.CompanyId != userCompanyId)
            {
                return new Response<DetailOrderDto>()
                {
                    Message = "O pedido não foi encontrado.",
                    Succeeded = false
                };
            }

            DetailOrderDto detailOrderDto = DetailOrderDto.Map(order);

            return new Response<DetailOrderDto>()
            {
                Data = detailOrderDto,
                Message = "Pedido encontrado com sucesso.",
                Succeeded = true
            };
        }

        public async Task<PagedResponse<IEnumerable<GetOrderDto>>> GetOrdersByDateRangePagedAsync(int companyId, RequestParameter parameter)
        {
            Company company = await _companyRepository.GetByIdAsync(companyId);
            if (company is null)
            {
                return new()
                {
                    Message = "Empresa não encontrada. Verifique e tente novamente.",
                    Succeeded = false
                };
            }

            var orders = await _orderRepository.GetOrdersByDateRangePagedAsync(
                companyId,
                parameter.InitialDate,
                parameter.FinalDate,
                parameter.PageSize,
                parameter.PageNumber);

            IEnumerable<GetOrderDto> mappedOrders = GetOrderDto.Map(orders.orders);

            // T data, int pageNumber, int pageSize, int totalItems = 0
            return new(
                data: mappedOrders,
                pageNumber: parameter.PageNumber,
                pageSize: parameter.PageSize,
                totalItems: orders.count
            );
        }

        public async Task<Response<IEnumerable<GetOrderDto>>> GetOrdersByCompanyAsync(int companyId)
        {
            if (companyId <= 0)
            {
                return new()
                {
                    Message = "Empresa não encontrada. Verifique e tente novamente.",
                    Succeeded = false
                };
            }

            IEnumerable<Order> orders = await _orderRepository.GetOrdersByCompanyAsync(companyId);
            IEnumerable<GetOrderDto> mappedOrders = GetOrderDto.Map(orders);

            return new()
            {
                Data = mappedOrders,
                Message = "Pedidos encontrados com sucesso.",
                Succeeded = true
            };
        }

        public async Task<PagedResponse<IEnumerable<GetOrderDto>>> GetOrdersByDateAsync(int companyId, DateTime? date)
        {
            if (companyId <= 0)
            {
                return new()
                {
                    Message = "Empresa não encontrada. Verifique e tente novamente.",
                    Succeeded = false
                };
            }

            if (date == null)
            {
                // Date never goes null to repository
                date = DateTime.Today.Date;
            }

            IEnumerable<Order> orders = await _orderRepository.GetOrdersByDateAsync(companyId, date);
            IEnumerable<GetOrderDto> getOrderDtoCollection = GetOrderDto.Map(orders);

            return new()
            {
                Data = getOrderDtoCollection,
                Message = "Pedidos encontrados com sucesso.",
                Succeeded = true
            };
        }

        public async Task<Response<IEnumerable<GetOrderDto>>> GetOrdersByDateRangeAsync(int companyId, DateTime? initialDate, DateTime? finalDate)
        {
            ICollection<Order> orders = await _orderRepository.GetOrdersByDateRangeAsync(companyId, initialDate, finalDate);
            IEnumerable<GetOrderDto> getOrderDtoCollection = GetOrderDto.Map(orders);

            return new()
            {
                Message = "Pedidos encontrados com sucesso",
                Succeeded = true,
                Data = getOrderDtoCollection
            };
        }

        public async Task<Response<UpdateOrderDto>> MakeOrderPaymentAsync(int orderId, int userCompanyId)
        {
            Order order = await _orderRepository.GetOrderDetailsAsync(orderId);

            if (order.CompanyId != userCompanyId)
            {
                return new Response<UpdateOrderDto>()
                {
                    Message = "Este pedido não pertence a sua empresa. Verifique e tente novamente.",
                    Succeeded = false
                };
            }

            order.MarkAsPaid(userCompanyId);
            await _orderRepository.UpdateAsync(order);

            return new Response<UpdateOrderDto>()
            {
                Message = "Pedido atualizado com sucesso.",
                Succeeded = true
            };
        }

        public async Task<Response<GetOrderDto>> DeleteOrderAsync(int orderId, int userCompanyId)
        {
            Order order = await _orderRepository.GetByIdAsync(orderId);

            if (order.CompanyId != userCompanyId)
            {
                return new Response<GetOrderDto>()
                {
                    Message = "Este pedido não pertence a sua empresa. Verifique e tente novamente.",
                    Succeeded = false
                };
            }

            await _orderRepository.DeleteAsync(order);

            return new Response<GetOrderDto>()
            {
                Message = "Pedido deletado com sucesso.",
                Succeeded = true
            };
        }

        public async Task<Response<GetCreateOrderItemsDto>> GetCreateOrdersItemsAsync(int companyId)
        {
            ICollection<Customer> customers = await _customerRepository.GetActiveCustomersByCompanyAsync(companyId);
            ICollection<Meal> meals = await _mealRepository.GetMealsByCompanyAsync(companyId);
            ICollection<PaymentType> paymentTypes = await _paymentTypeRepository.GetAllAsync();

            GetCreateOrderItemsDto orderItemsDto = new()
            {
                Customers = customers,
                Meals = meals,
                PaymentTypes = paymentTypes
            };

            return new Response<GetCreateOrderItemsDto>()
            {
                Message = "Sucesso.",
                Succeeded = true,
                Data = orderItemsDto
            };
        }
    }
}