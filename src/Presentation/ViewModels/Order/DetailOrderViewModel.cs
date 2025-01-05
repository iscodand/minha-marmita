using Application.Dtos.Order;

namespace Presentation.ViewModels.Order
{
    public class DetailOrderViewModel
    {
        public DetailOrderDto Order { get; set; }
        public UpdateOrderViewModel UpdateOrder { get; set; }
        public CreateOrderDropdown Dropdown { get; set; }
    }
}