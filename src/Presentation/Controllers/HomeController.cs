using Application.Contracts.Services;
using Application.Dtos.Data;
using Application.DTOs.Authentication;
using Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Common;

namespace Presentation.Controllers
{
    [Route("")]
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IDataService _dataService;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;

        public HomeController(IDataService dataService, IOrderService orderService, IUserService userService)
        {
            _dataService = dataService;
            _orderService = orderService;
            _userService = userService;
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
        [Route("eu")]
        public IActionResult MyProfile()
        {
            return View();
        }

        [HttpPost]
        [Route("meu-perfil/atualizar")]
        public async Task<IActionResult> UpdateMyProfile(UpdateUserViewModel request)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();

                UpdateUserDto updateUserDto = new()
                {
                    Name = request.Name,
                    Username = request.Username,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    CompanyId = authenticatedUser.CompanyId
                };

                Response<UpdateUserDto> result = await _userService.UpdateUserAsync(updateUserDto);

                TempData["Message"] = result.Message;
                TempData["Succeeded"] = result.Succeeded;

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(MyProfile));
                }
            }

            return RedirectToAction(nameof(MyProfile));
        }

        [HttpPost]
        [Route("meu-perfil/atualizar")]
        public async Task<IActionResult> UpdateMyProfile(UpdateUserViewModel request)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();

                UpdateUserDto updateUserDto = new()
                {
                    Name = request.Name,
                    Username = request.Username,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    CompanyId = authenticatedUser.CompanyId
                };

                Response<UpdateUserDto> result = await _userService.UpdateUserAsync(updateUserDto);

                TempData["Message"] = result.Message;
                TempData["Succeeded"] = result.Succeeded;

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(MyProfile));
                }
            }

            return RedirectToAction(nameof(MyProfile));
        }
    }
}