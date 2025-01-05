using Application.Dtos.Meal;
using Application.Parameters;
using Application.Wrappers;

namespace Application.Contracts.Services
{
    public interface IMealService
    {
        public Task<Response<CreateMealDto>> CreateMealAsync(CreateMealDto createMealDto);
        public Task<Response<IEnumerable<GetMealDto>>> GetMealsByCompanyAsync(int companyId);
        public Task<PagedResponse<IEnumerable<GetMealDto>>> GetByCompanyPagedAsync(int companyId, RequestParameter parameter);
        public Task<Response<GetMealDto>> GetMealByIdAsync(int mealId, int userCompanyId);
        public Task<Response<IEnumerable<GetMealDto>>> GetMealsByDateRangeAsync(int userCompanyId, DateTime initialDate, DateTime finalDate);
        public Task<Response<IEnumerable<GetMealDto>>> SearchByMealAsync(string name);
        public Task<Response<DetailMealDto>> DetailMealAsync(int mealId, int userCompanyId);
        public Task<Response<GetMealDto>> UpdateMealAsync(UpdateMealDto updateMealDto);
        public Task<Response<GetMealDto>> DeleteMealAsync(DeleteMealDto deleteMealDto);
    }
}