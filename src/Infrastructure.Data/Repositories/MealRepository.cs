using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Data.DataContext;

namespace Infrastructure.Data.Repositories
{
    public class MealRepository : GenericRepository<Meal>, IMealRepository
    {
        private readonly DbSet<Meal> _meals;

        public MealRepository(ApplicationDbContext context) : base(context)
        {
            _meals = context.Meals;
        }

        public async Task<Meal> DetailMealAsync(int mealId)
        {
            return await _meals.AsNoTracking()
                                .Include(x => x.User).AsNoTracking()
                                .Include(x => x.Company).AsNoTracking()
                                .Include(x => x.Orders)
                                .ThenInclude(x => x.Customer).AsNoTracking()
                                .Include(x => x.Orders)
                                .ThenInclude(x => x.PaymentType).AsNoTracking()
                                .Where(x => x.Id == mealId).AsNoTracking()
                                .FirstOrDefaultAsync()
                                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Meal>> SearchByMealAsync(string name)
        {
            return await _meals.AsNoTracking()
                               .Where(x => x.Description.ToUpper().Trim().Contains(name.ToUpper().Trim()))
                               .ToListAsync()
                               .ConfigureAwait(false);
        }

        public async Task<(IEnumerable<Meal> meals, int count)> GetByCompanyPagedAsync(int companyId, int pageNumber, int pageSize)
        {
            IEnumerable<Meal> meals = await _meals.AsNoTracking()
                            .Include(x => x.User)
                            .Include(x => x.Company)
                            .Include(x => x.Orders)
                            .Where(x => x.CompanyId == companyId)
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync()
                            .ConfigureAwait(false);

            int count = await _meals.AsNoTracking()
                                    .Where(x => x.CompanyId == companyId)
                                    .CountAsync()
                                    .ConfigureAwait(false);

            return (meals, count);
        }

        public async Task<ICollection<Meal>> GetMealsByCompanyAsync(int companyId)
        {
            return await _meals.AsNoTracking()
                               .Include(x => x.User).AsNoTracking()
                               .Include(x => x.Company).AsNoTracking()
                               .Include(x => x.Orders).AsNoTracking()
                               .Where(x => x.CompanyId == companyId).AsNoTracking()
                               .ToListAsync()
                               .ConfigureAwait(false);
        }

        public async Task<ICollection<Meal>> GetMealsByDateRangeAsync(int companyId, DateTime initialDate, DateTime finalDate)
        {
            return await _meals.AsNoTracking()
                               .Include(x => x.Orders.Where(x => x.CreatedAt.Date >= initialDate.Date && x.CreatedAt.Date <= finalDate.Date))
                               .Where(x => x.CompanyId == companyId)
                               .ToListAsync()
                               .ConfigureAwait(false);
        }

        public async Task<bool> MealExistsByDescriptionAsync(string description, int companyId)
        {
            return await _meals.AsNoTracking()
                               .Where(x => x.CompanyId == companyId).AsNoTracking()
                               .AnyAsync(x => x.Description == description)
                               .ConfigureAwait(false);
        }
    }
}