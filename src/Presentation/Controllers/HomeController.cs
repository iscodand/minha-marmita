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

        public HomeController(IDataService dataService)
        {
            _dataService = dataService;
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
    }
}