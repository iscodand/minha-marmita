using Application.Dtos.Customer;
using Application.Dtos.Order;
using Presentation.ViewModels.Order;

namespace Presentation.ViewModels.Customer
{
    public class DetailCustomerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public IEnumerable<GetOrderDto> Orders { get; set; }

        public static DetailCustomerViewModel Map(GetCustomerDto customer)
        {
            return new()
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                IsActive = customer.IsActive,
                Orders = customer.Orders
            };
        }
    }
}