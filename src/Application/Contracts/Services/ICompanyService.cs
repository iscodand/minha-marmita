using Application.DTOs.Company.Requests;
using Application.DTOs.Company.Response;
using Application.Wrappers;

namespace Application.Contracts.Services
{
    public interface ICompanyService
    {
        public Task<Response<string>> CreateCompanyAsync(CreateCompanyRequest request);
        public Task<Response<IEnumerable<CompanyDTO>>> GetCompaniesAsync();
        public Task<Response<DetailCompanyDTO>> GetCompanyByIdAsync(int companyId);
    }
}