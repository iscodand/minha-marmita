namespace Application.Dtos.Order
{
    public class GetOrderDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaidAt { get; set; }
        public string PaymentType { get; set; }
        public string Meal { get; set; }
        public string Customer { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public static IEnumerable<GetOrderDto> Map(IEnumerable<Domain.Entities.Order> orders)
        {
            return orders.Select(x => new GetOrderDto
            {
                Id = x.Id,
                Description = x.Description,
                Price = x.Price,
                IsPaid = x.IsPaid,
                PaidAt = x.PaidAt,
                PaymentType = (x.PaymentType is not null) ? x.PaymentType.Description : "",
                Meal = (x.Meal is not null) ? x.Meal.Description : "Não Identificado",
                Customer = (x.Customer is not null) ? x.Customer.Name : "Não Identificado" ,
                CreatedBy = "",
                CreatedAt = x.CreatedAt,
            });;
        }
    }
}