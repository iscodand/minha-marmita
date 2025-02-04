using Application.Dtos.Order;
using Application.Dtos.User;

namespace Presentation.ViewModels.User
{
    public class MyProfileViewModel
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string PhoneNumber { get; set; }

        public GetUserDto User { get; set; }
        public IEnumerable<GetOrderDto> Orders { get; set; } = new List<GetOrderDto>();

        public static MyProfileViewModel Map(GetUserDto user, IEnumerable<GetOrderDto> orders)
        {
            return new()
            {
                User = user,
                Orders = orders
            };
        }
    }
}