using Domain.Entities;

namespace Application.Contracts.Repositories
{
    public interface ICompanyRepository : IGenericRepository<Company>
    {
        public Task<IEnumerable<Company>> GetCompaniesPagedAsync(int pageNumber, int pageSize);
        public Task<bool> CompanyAlreadyRegisteredByCNPJAsync(string cnpj);
        public Task<Company> DetailByIdAsync(int id);
    }
}