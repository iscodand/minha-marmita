using Domain.Entities;
using Application.Contracts.Repositories;

namespace Infrastructure.Data.Repositories
{
    public interface IMealRepository : IGenericRepository<Meal>
    {
        public Task<ICollection<Meal>> GetMealsByCompanyAsync(int companyId);
        public Task<(IEnumerable<Meal> meals, int count)> GetByCompanyPagedAsync(int companyId, int pageNumber, int pageSize);
        public Task<ICollection<Meal>> GetMealsByDateRangeAsync(int companyId, DateTime initialDate, DateTime finalDate);
        public Task<IEnumerable<Meal>> SearchByMealAsync(string name);
        public Task<bool> MealExistsByDescriptionAsync(string description, int companyId);
        public Task<Meal> DetailMealAsync(int mealId);
    }
}