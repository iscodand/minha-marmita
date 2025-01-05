using Application.Dtos.Customer;
using Application.Parameters;
using Application.Wrappers;

namespace Application.Contracts.Services
{
        public interface ICustomerService
        {
                public Task<PagedResponse<IEnumerable<GetCustomerDto>>> GetCustomersByCompanyAsync(int companyId, RequestParameter parameter);
                public Task<Response<IEnumerable<GetCustomerDto>>> GetCustomersByDateRangeAsync(int userCompanyId, DateTime initialDate, DateTime finalDate);
                public Task<Response<GetCustomerDto>> GetCustomerByIdAsync(int customerId, int userCompanyId);
                public Task<Response<IEnumerable<GetCustomerDto>>> SearchByCustomerAsync(string name);
                public Task<Response<DetailCustomerDto>> DetailCustomerAsync(int customerId, int userCompanyId);
                public Task<Response<CreateCustomerDto>> CreateCustomerAsync(CreateCustomerDto createCustomerDto);
                public Task<Response<UpdateCustomerDto>> UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto);
                public Task<Response<UpdateCustomerDto>> ActivateCustomerAsync(int customerId, int userCompanyId);
                public Task<Response<UpdateCustomerDto>> InactivateCustomerAsync(int customerId, int userCompanyId);
                public Task<Response<GetCustomerDto>> DeleteCustomerAsync(int customerId, int userCompanyId);
        }
}