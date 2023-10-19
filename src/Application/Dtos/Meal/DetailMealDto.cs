using Application.Dtos.Order;

namespace Application.Dtos.Meal
{
    public class DetailMealDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Accompaniments { get; set; }
        public string CreatedBy { get; set; }
        public ICollection<GetOrderDto> Orders { get; set; }
    }
}