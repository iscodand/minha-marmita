using Application.Dtos.Meal;
using Application.Dtos.Order;

namespace Presentation.ViewModels.Meal
{
    public class DetailMealViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Accompaniments { get; set; }
        public string CreatedBy { get; set; }
        public IEnumerable<GetOrderDto> Orders { get; set; }

        public static DetailMealViewModel Map(GetMealDto dto)
        {
            return new()
            {
                Id = dto.Id,
                Description = dto.Description,
                Accompaniments = dto.Accompaniments,
                CreatedBy = dto.CreatedBy,
                Orders = dto.Orders
            };
        }
    }
}