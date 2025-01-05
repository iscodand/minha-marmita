using Application.Contracts.Repositories;
using Domain.Entities;
using Infrastructure.Data.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        private readonly DbSet<Company> _companies;

        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
            _companies = context.Companies;
        }

        public async Task<bool> CompanyAlreadyRegisteredByCNPJAsync(string cnpj)
        {
            return await _companies.AsNoTracking()
                                .Where(x => x.CNPJ == cnpj)
                                .AnyAsync()
                                .ConfigureAwait(false);
        }

        public async Task<Company> DetailByIdAsync(int id)
        {
            return await _companies.AsNoTracking()
                                .Where(x => x.Id == id)
                                .Include(x => x.Users)
                                .FirstOrDefaultAsync();
        }
    }
}