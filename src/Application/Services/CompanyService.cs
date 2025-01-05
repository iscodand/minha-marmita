using Application.Contracts.Repositories;
using Application.Contracts.Services;
using Application.Dtos.User;
using Application.DTOs.Company.Requests;
using Application.DTOs.Company.Response;
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
                return new Response<string>()
                {
                    Succeeded = false,
                    Message = "Uma empresa já está cadastrada com esse CNPJ.",
                    Data = null
                };
            }

            Company company = new()
            {
                Name = request.Name,
                CNPJ = request.CNPJ
            };

            await _companyRepository.CreateAsync(company);

            return new Response<string>()
            {
                Succeeded = true,
                Message = "Empresa cadastrada com sucesso.",
                Data = null
            };
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

        public async Task<Response<IEnumerable<CompanyDTO>>> GetCompaniesAsync()
        {
            IEnumerable<Company> companies = await _companyRepository.GetAllAsync();
            IEnumerable<CompanyDTO> companyDTO = CompanyDTO.Map(companies);

            return new Response<IEnumerable<CompanyDTO>>()
            {
                Succeeded = true,
                Message = "Empresas recuperadas com sucesso.",
                Data = companyDTO
            };
        }
    }
}