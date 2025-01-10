using Application.DTOs.Company.Requests;
using Application.DTOs.Company.Response;
using Application.Parameters;
using Application.Wrappers;

namespace Application.Contracts.Services
{
    public interface ICompanyService
    {
        public Task<PagedResponse<IEnumerable<CompanyDTO>>> GetCompaniesAsync(RequestParameter parameter);
        public Task<Response<string>> CreateCompanyAsync(CreateCompanyRequest request);
        public Task<Response<DetailCompanyDTO>> GetCompanyByIdAsync(int companyId);
        public Task<Response<string>> DeleteAsync(int companyId);
    }
}