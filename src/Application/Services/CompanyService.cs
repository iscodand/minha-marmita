using Application.Contracts.Repositories;
using Application.Contracts.Services;
using Application.DTOs.Company.Requests;
using Application.DTOs.Company.Response;
using Application.Parameters;
using Application.Wrappers;
using Domain.Entities;

namespace Application.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        // todo => adicionar validação para criação de nova empresa
        public async Task<Response<string>> CreateCompanyAsync(CreateCompanyRequest request)
        {
            bool companyAlreadyRegistered = await _companyRepository.CompanyAlreadyRegisteredByCNPJAsync(request.CNPJ);
            if (companyAlreadyRegistered)
            {
                return Response<string>.Failure(
                    new List<string> { "Uma empresa já está cadastrada com esse CNPJ." }
                );
            }

            Company company = new()
            {
                Name = request.Name,
                CNPJ = request.CNPJ
            };

            company = await _companyRepository.CreateAsync(company);

            return Response<string>.Success(
                message: "Empresa cadastrada com sucesso.",
                data: company.Id.ToString()
            );
        }

        public async Task<Response<DetailCompanyDTO>> GetCompanyByIdAsync(int companyId)
        {
            // TODO => adicionar pedidos nos detalhes da empresa
            Company company = await _companyRepository.DetailByIdAsync(companyId);
            if (company is null)
            {
                return new()
                {
                    Message = "Empresa não encontrada.",
                    Succeeded = false,
                    Data = null
                };
            }

            DetailCompanyDTO mappedCompany = DetailCompanyDTO.Map(company);

            return new()
            {
                Message = "Empresa recuperada com sucesso.",
                Succeeded = true,
                Data = mappedCompany
            };
        }

        public async Task<PagedResponse<IEnumerable<CompanyDTO>>> GetCompaniesAsync(RequestParameter parameter)
        {
            IEnumerable<Company> companies = await _companyRepository.GetCompaniesPagedAsync(parameter.PageNumber, parameter.PageSize);
            IEnumerable<CompanyDTO> mappedCompanies = CompanyDTO.Map(companies);

            return new PagedResponse<IEnumerable<CompanyDTO>>()
            {
                Succeeded = true,
                Message = "Empresas recuperadas com sucesso.",
                Data = mappedCompanies
            };
        }

        public async Task<Response<string>> DeleteAsync(int companyId)
        {
            Company company = await _companyRepository.GetByIdAsync(companyId);
            if (company is null)
            {
                return Response<string>.Failure(
                    errors: new List<string> { "Empresa não encontrada." }
                );
            }

            await _companyRepository.DeleteAsync(company);

            return Response<string>.Success(
                message: "Empresa deletada com sucesso.",
                data: string.Empty
            );
        }
    }
}