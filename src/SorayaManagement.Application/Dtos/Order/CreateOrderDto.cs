using System.ComponentModel.DataAnnotations;
namespace SorayaManagement.Application.Dtos
{
    public class CreateOrderDto
    {
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsPaid { get; set; }
        public DateTime PaidAt { get; set; }
        public int PaymentTypeId { get; set; }
        public int MealId { get; set; }
        public int CustomerId { get; set; }
    }
}