using Application.Dtos.Order;
using Application.Dtos.User;

namespace Presentation.ViewModels.User
{
    public class MyProfileViewModel
    {
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