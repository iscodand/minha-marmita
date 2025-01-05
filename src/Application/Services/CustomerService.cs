using Application.Dtos.Customer;
using Application.Dtos.Order;
using Application.Wrappers;
using Domain.Entities;
using Application.Contracts.Repositories;
using Application.Contracts.Services;
using Application.Parameters;
using Microsoft.AspNetCore.Authorization;

namespace Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Response<CreateCustomerDto>> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            if (createCustomerDto == null)
            {
                return new Response<CreateCustomerDto>()
                {
                    Message = "Cliente não pode ser nulo.",
                    Succeeded = false
                };
            }

            if (await _customerRepository.CustomerExistsByCompanyAsync(createCustomerDto.Name, createCustomerDto.CompanyId))
            {
                return new Response<CreateCustomerDto>()
                {
                    Message = "Um cliente com esse nome já foi cadastrado. Verifique e tente novamente",
                    Succeeded = false
                };
            }

            Customer customer = Customer.Create(
                createCustomerDto.Name,
                createCustomerDto.Phone,
                createCustomerDto.CompanyId,
                createCustomerDto.UserId
            );

            await _customerRepository.CreateAsync(customer);

            return new Response<CreateCustomerDto>()
            {
                Message = "Cliente criado com sucesso",
                Succeeded = true
            };
        }

        public async Task<Response<IEnumerable<GetCustomerDto>>> SearchByCustomerAsync(string name)
        {
            ICollection<Customer> customers = await _customerRepository.SearchByCustomerAsync(name);

            IEnumerable<GetCustomerDto> mappedCustomers = GetCustomerDto.Map(customers);

            return new Response<IEnumerable<GetCustomerDto>>()
            {
                Data = mappedCustomers,
                Message = "Clientes recuperados com sucesso.",
                Succeeded = true
            };
        }

        public async Task<Response<UpdateCustomerDto>> UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto)
        {
            if (updateCustomerDto == null)
            {
                return new Response<UpdateCustomerDto>()
                {
                    Message = "Cliente não pode ser nulo.",
                    Succeeded = false
                };
            }

            //if (await _customerRepository.CustomerExistsByCompanyAsync(updateCustomerDto.Name, updateCustomerDto.UserCompanyId))
            //{
            //    return new Response<UpdateCustomerDto>()
            //    {
            //        Message = "Um cliente com esse nome já foi cadastrado. Verifique e tente novamente",
            //        Succeeded = false
            //    };
            //}

            Customer customer = await _customerRepository.GetByIdAsync(updateCustomerDto.Id);

            if (customer.CompanyId != updateCustomerDto.UserCompanyId)
            {
                return new Response<UpdateCustomerDto>()
                {
                    Message = "Este cliente não pertence a sua empresa. Verifique e tente novamente.",
                    Succeeded = false
                };
            }

            customer.Update(
                updateCustomerDto.Name,
                updateCustomerDto.Phone
            );

            await _customerRepository.UpdateAsync(customer);

            return new Response<UpdateCustomerDto>()
            {
                Message = "Cliente atualizado com sucesso",
                Succeeded = true
            };
        }

        public async Task<Response<GetCustomerDto>> GetCustomerByIdAsync(int customerId, int userCompanyId)
        {
            Customer customer = await _customerRepository.DetailCustomerAsync(customerId);

            if (userCompanyId != customer.CompanyId)
            {
                return new Response<GetCustomerDto>()
                {
                    Message = "Este cliente não pertence a sua empresa. Verifique e tente novamente.",
                    Succeeded = false
                };
            }

            GetCustomerDto mappedCustomer = GetCustomerDto.Map(customer);

            return new Response<GetCustomerDto>()
            {
                Message = "Cliente encontrado com sucesso.",
                Succeeded = true,
                Data = mappedCustomer
            };
        }

        public async Task<PagedResponse<IEnumerable<GetCustomerDto>>> GetCustomersByCompanyAsync(int companyId, RequestParameter parameter)
        {
            var customers = await _customerRepository.GetByCompanyPagedAsync(
                companyId,
                parameter.PageNumber,
                parameter.PageSize
            );
            IEnumerable<GetCustomerDto> mappedCustomers = GetCustomerDto.Map(customers.customers);

            return new(
                data: mappedCustomers,
                pageNumber: parameter.PageNumber,
                pageSize: parameter.PageSize,
                totalItems: customers.count
            );
        }

        public async Task<Response<IEnumerable<GetCustomerDto>>> GetCustomersByDateRangeAsync(int userCompanyId, DateTime initialDate, DateTime finalDate)
        {
            IEnumerable<Customer> customers = await _customerRepository.GetCustomersByDateRangeAsync(userCompanyId, initialDate, finalDate);
            IEnumerable<GetCustomerDto> mappedCustomers = GetCustomerDto.Map(customers);

            return new()
            {
                Message = "Clientes encontrados com sucesso.",
                Succeeded = true,
                Data = mappedCustomers
            };
        }

        public async Task<Response<DetailCustomerDto>> DetailCustomerAsync(int customerId, int userCompanyId)
        {
            Customer customer = await _customerRepository.DetailCustomerAsync(customerId);

            // iscodand - 16/10/23 => Filtering orders by current month
            ICollection<Order> customerOrders = customer.Orders.Where(x => x.CreatedAt.Month == DateTime.Now.Month).ToList();

            List<GetOrderDto> getOrderDtoCollection = new();
            foreach (Order order in customerOrders)
            {
                GetOrderDto getOrderDto = new()
                {
                    Id = order.Id,
                    Description = order.Description,
                    Price = order.Price,
                    IsPaid = order.IsPaid,
                    PaidAt = order.PaidAt,
                    PaymentType = order.PaymentType.Description,
                    Meal = order.Meal.Description,
                    Customer = order.Customer.Name,
                    CreatedAt = order.CreatedAt
                };

                getOrderDtoCollection.Add(getOrderDto);
            }

            DetailCustomerDto detailCustomerDto = new()
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                IsActive = customer.IsActive,
                CreatedBy = customer.User.Name,
                Orders = getOrderDtoCollection
            };

            if (userCompanyId != customer.CompanyId)
            {
                return new Response<DetailCustomerDto>()
                {
                    Message = "Este cliente não pertence a sua empresa. Verifique e tente novamente.",
                    Succeeded = false
                };
            }

            return new Response<DetailCustomerDto>()
            {
                Message = "Cliente encontrado com sucesso",
                Succeeded = true,
                Data = detailCustomerDto
            };
        }

        public async Task<Response<UpdateCustomerDto>> InactivateCustomerAsync(int customerId, int userCompanyId)
        {
            Customer customer = await _customerRepository.GetByIdAsync(customerId);

            if (!customer.IsActive)
            {
                return new Response<UpdateCustomerDto>()
                {
                    Message = "Cliente já está inativo",
                    Succeeded = false
                };
            }

            if (customer.CompanyId != userCompanyId)
            {
                return new Response<UpdateCustomerDto>()
                {
                    Message = "Você não pode desativar clientes de outras empresas.",
                    Succeeded = false
                };
            }

            customer.Inactivate();

            await _customerRepository.UpdateAsync(customer);

            return new Response<UpdateCustomerDto>()
            {
                Message = "Cliente desativado com sucesso",
                Succeeded = true
            };
        }

        public async Task<Response<UpdateCustomerDto>> ActivateCustomerAsync(int customerId, int userCompanyId)
        {
            Customer customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer.IsActive)
            {
                return new Response<UpdateCustomerDto>()
                {
                    Message = "Cliente já está ativo",
                    Succeeded = false
                };
            }

            if (customer.CompanyId != userCompanyId)
            {
                return new Response<UpdateCustomerDto>()
                {
                    Message = "Você não pode ativar clientes de outras empresas.",
                    Succeeded = false
                };
            }

            customer.Activate();

            await _customerRepository.UpdateAsync(customer);

            return new Response<UpdateCustomerDto>()
            {
                Message = "Cliente ativado com sucesso",
                Succeeded = true
            };
        }

        public async Task<Response<GetCustomerDto>> DeleteCustomerAsync(int customerId, int userCompanyId)
        {
            Customer customer = await _customerRepository.DetailCustomerAsync(customerId);

            if (customer.CompanyId != userCompanyId)
            {
                return new Response<GetCustomerDto>()
                {
                    Message = "Você não pode excluir clientes de outras empresas.",
                    Succeeded = false
                };
            }

            if (customer.Orders.Any())
            {
                return new Response<GetCustomerDto>()
                {
                    Message = "Você não pode excluir esse cliente pois ele possui pedidos cadastrados.",
                    Succeeded = false
                };
            }

            await _customerRepository.DeleteAsync(customer);

            return new Response<GetCustomerDto>()
            {
                Message = "Cliente deletado com sucesso.",
                Succeeded = true
            };
        }
    }
}