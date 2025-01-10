using Application.Contracts.Services;
using Application.Dtos.Meal;
using Application.DTOs.Authentication;
using Application.Parameters;
using Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Common;
using Presentation.ViewModels.Meal;

namespace Presentation.Controllers
{
    [Route("sabores/")]
    [Authorize]
    public class MealController : BaseController
    {
        private readonly IMealService _mealService;

        public MealController(IMealService mealService)
        {
            _mealService = mealService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Meals(int pageNumber = 1)
        {
            RequestParameter parameters = new()
            {
                PageNumber = pageNumber,
                PageSize = 10,
            };

            GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();
            var meals = await _mealService.GetByCompanyPagedAsync(
                authenticatedUser.CompanyId,
                parameters
            );

            return View(meals);
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> SearchByMealAsync([FromQuery] string name)
        {
            var result = await _mealService.SearchByMealAsync(name);
            return Json(result);
        }

        [HttpGet]
        [Route("novo/")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("novo/")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMealDto request)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();

                request.UserId = authenticatedUser.Id;
                request.CompanyId = authenticatedUser.CompanyId;

                Response<CreateMealDto> result = await _mealService.CreateMealAsync(request);

                ViewData["Message"] = result.Message;

                if (result.Succeeded)
                {
                    ViewData["Succeeded"] = true;
                    return RedirectToAction(nameof(Meals));
                }

                return View();
            }

            return RedirectToAction(nameof(Meals));
        }

        [HttpGet]
        [Route("{mealId}")]
        public async Task<IActionResult> Update(int mealId)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();
                Response<GetMealDto> result = await _mealService.GetMealByIdAsync(mealId, authenticatedUser.CompanyId);

                if (result.Succeeded)
                {
                    UpdateMealViewModel updateMealViewModel = new()
                    {
                        Id = result.Data.Id,
                        Description = result.Data.Description,
                        Accompaniments = result.Data.Accompaniments,
                        UserCompanyId = authenticatedUser.CompanyId,
                        Meal = DetailMealViewModel.Map(result.Data),
                    };

                    return View(updateMealViewModel);
                }
            }

            return RedirectToAction(nameof(Meals));
        }

        [HttpPost]
        [Route("{mealId}")]
        public async Task<IActionResult> Update(int mealId, UpdateMealViewModel updateMealViewModel)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();

                UpdateMealDto updateMealDto = new()
                {
                    Id = mealId,
                    Description = updateMealViewModel.Description,
                    Accompaniments = updateMealViewModel.Accompaniments,
                    UserCompanyId = authenticatedUser.CompanyId
                };

                Response<GetMealDto> result = await _mealService.UpdateMealAsync(updateMealDto);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Meals));
                }
            }

            return View(updateMealViewModel);
        }

        [HttpPost]
        [Route("deletar/{mealId}")]
        public async Task<IActionResult> Delete(int mealId)
        {
            GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();

            DeleteMealDto deleteMealDto = new()
            {
                Id = mealId,
                UserCompanyId = authenticatedUser.CompanyId
            };

            Response<GetMealDto> result = await _mealService.DeleteMealAsync(deleteMealDto);

            ViewData["Message"] = result.Message;

            if (result.Succeeded)
            {
                ViewData["Succeeded"] = true;
                return RedirectToAction(nameof(Meals));
            }

            return View();
        }
    }
}