namespace Application.Dtos.Meal
{
    public class GetMealDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Accompaniments { get; set; }
        public string CreatedBy { get; set; }
        public int OrdersCount { get; set; }

        public static IEnumerable<GetMealDto> Map(IEnumerable<Domain.Entities.Meal> meals)
        {
            return meals.Select(m => new GetMealDto
            {
                Id = m.Id,
                Description = m.Description,
                Accompaniments = m.Accompaniments,
                CreatedBy = "teste",
                OrdersCount = m.Orders.Count,
            });
        }
    }
}