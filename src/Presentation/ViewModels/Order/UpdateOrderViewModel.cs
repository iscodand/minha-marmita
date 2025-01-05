using Application.Dtos.Order;

namespace Presentation.ViewModels.Order
{
    public class UpdateOrderViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int CustomerId { get; set; }
        public int MealId { get; set; }
        public int PaymentTypeId { get; set; }

        public bool IsPaid { get; set; }
        public DateTime? PaidAt { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public CreateOrderDropdown CreateOrderDropdown { get; set; }

        public static UpdateOrderViewModel Map(DetailOrderDto dto, CreateOrderDropdown dropdown)
        {
            return new()
            {
                Id = dto.Id,
                Description = dto.Description,
                Price = dto.Price,
                CustomerId = dto.CustomerId,
                MealId = dto.MealId,
                PaymentTypeId = dto.PaymentTypeId,
                IsPaid = dto.IsPaid,
                PaidAt = dto.PaidAt,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,

                CreateOrderDropdown = dropdown
            };
        }
    }
}