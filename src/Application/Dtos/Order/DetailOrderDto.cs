using Domain.Entities;

namespace Application.Dtos.Order
{
    public class DetailOrderDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaidAt { get; set; }
        public int PaymentTypeId { get; set; }
        public string PaymentType { get; set; }
        public int MealId { get; set; }
        public string Meal { get; set; }
        public int CustomerId { get; set; }
        public string Customer { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static DetailOrderDto Map(Domain.Entities.Order order)
        {
            return new()
            {
                Id = order.Id,
                Description = order.Description,
                Price = order.Price,
                IsPaid = order.IsPaid,
                PaidAt = order.PaidAt,
                PaymentType = order.PaymentType.Description,
                PaymentTypeId = order.PaymentType.Id,
                MealId = order.Meal.Id,
                Meal = order.Meal.Description,
                CustomerId = order.Customer.Id,
                Customer = order.Customer.Name,
                CreatedBy = order.User.Name,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt
            };
        }
    }
}