using Application.Contracts.Services;
using Application.Dtos.Order;
using Application.Dtos.User;
using Application.DTOs.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Common;
using Presentation.ViewModels.User;

namespace Presentation.Controllers
{
    [Route("")]
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IDataService _dataService;
        private readonly IOrderService _orderService;

        public HomeController(IDataService dataService, IOrderService orderService)
        {
            _dataService = dataService;
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Home([FromQuery] string date)
        {
            GetAuthenticatedUserDto authenticatedUser = await AuthenticatedUser.GetAuthenticatedUserAsync();
            SessionService.AddUserSession(authenticatedUser);

            if (authenticatedUser is not null)
            {
                DateTime today = DateTime.Today.Date;
                DateTime dateFilter = DateTime.Today.Date;

                if (date == "today")
                {
                    dateFilter = DateTime.Today.Date;
                }
                else if (date == "lastMonth")
                {
                    dateFilter = DateTime.Today.Date.AddMonths(-1);
                }
                else
                {
                    dateFilter = DateTime.Today.Date.AddDays(-7);
                }

                var result = await _dataService.GetDataAsync(authenticatedUser.CompanyId, dateFilter, today);

                return View(result.Data);
            }

            return RedirectToAction("Login", "Authentication");
        }

        [HttpGet]
        [Route("meu-perfil")]
        public async Task<IActionResult> MyProfile()
        {
            GetAuthenticatedUserDto authenticatedUser = await AuthenticatedUser.GetAuthenticatedUserAsync();
            SessionService.AddUserSession(authenticatedUser);

            GetUserDto user = new()
            {
                Id = authenticatedUser.Id,
                Name = authenticatedUser.Name,
                Email = authenticatedUser.Email,
                Username = authenticatedUser.Username,
                Role = authenticatedUser.Role,
                PhoneNumber = authenticatedUser.PhoneNumber
            };

            var orders = await _orderService.GetByUserIdAsync(authenticatedUser.Id);
            MyProfileViewModel viewModel = MyProfileViewModel.Map(user, orders.Data);

            return View(viewModel);
        }
    }
}