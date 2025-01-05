using Application.Dtos.Order;

namespace Application.Dtos.Customer
{
    public class GetCustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public int OrdersCount { get; set; }
        public IEnumerable<GetOrderDto> Orders { get; set; }

        public static GetCustomerDto Map(Domain.Entities.Customer customer)
        {
            return new()
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                IsActive = customer.IsActive,
                Orders = GetOrderDto.Map(customer.Orders)
            };
        }

        public static IEnumerable<GetCustomerDto> Map(IEnumerable<Domain.Entities.Customer> customers)
        {
            return customers.Select(x => new GetCustomerDto
            {
                Id = x.Id,
                Name = x.Name,
                Phone = x.Phone,
                IsActive = x.IsActive,
                Orders = GetOrderDto.Map(x.Orders),
                // CreatedBy = x.User is not null ? x.User.Name : "Não identificado.",
                OrdersCount = x.Orders.Count
            });
        }
    }
}