using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Dtos.Customer;
using Application.Wrappers;
using Presentation.ViewModels.Customer;
using Presentation.ViewModels.Order;
using Application.Dtos.Order;
using Presentation.Controllers.Common;
using Application.DTOs.Authentication;
using Application.Contracts.Services;
using Application.Parameters;

namespace Presentation.Controllers
{
    [Authorize]
    [Route("clientes/")]
    public class CustomerController : BaseController
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Customers(int pageNumber = 1)
        {
            RequestParameter parameters = new()
            {
                PageNumber = pageNumber,
                PageSize = 10,
            };

            GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();

            var result = await _customerService.GetCustomersByCompanyAsync(
                authenticatedUser.CompanyId,
                parameters
            );

            return View(result);
        }

        [HttpGet]
        [Route("buscar")]
        public async Task<IActionResult> SearchByCustomerAsync([FromQuery] string name)
        {
            var result = await _customerService.SearchByCustomerAsync(name);
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
        public async Task<IActionResult> Create(CreateCustomerViewModel createCustomerViewModel)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();

                CreateCustomerDto createCustomerDto = new()
                {
                    Name = createCustomerViewModel.Name,
                    Phone = createCustomerViewModel.Phone,
                    UserId = authenticatedUser.Id,
                    CompanyId = authenticatedUser.CompanyId
                };

                Response<CreateCustomerDto> response = await _customerService.CreateCustomerAsync(createCustomerDto);
                ViewData["Message"] = response.Message;

                if (response.Succeeded)
                {
                    ViewData["Succeeded"] = true;
                    return RedirectToAction(nameof(Customers));
                }
            }

            return View();
        }

        [HttpGet]
        [Route("{customerId}")]
        public async Task<IActionResult> Update(int customerId)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();
                Response<GetCustomerDto> result = await _customerService.GetCustomerByIdAsync(customerId, authenticatedUser.CompanyId);

                if (result.Succeeded)
                {
                    UpdateCustomerViewModel updateCustomerViewModel = new()
                    {
                        Id = result.Data.Id,
                        Name = result.Data.Name,
                        Phone = result.Data.Phone,
                        DetailCustomer = result.Data
                    };

                    return View(updateCustomerViewModel);
                }
            }

            return RedirectToAction(nameof(Customers));
        }

        [HttpPost]
        [Route("{customerId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int customerId, UpdateCustomerViewModel updateCustomerViewModel)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();

                UpdateCustomerDto updateCustomerDto = new()
                {
                    Id = customerId,
                    Name = updateCustomerViewModel.Name,
                    Phone = updateCustomerViewModel.Phone,
                    UserCompanyId = authenticatedUser.CompanyId
                };

                Response<UpdateCustomerDto> result = await _customerService.UpdateCustomerAsync(updateCustomerDto);

                ViewData["Message"] = result.Message;

                if (result.Succeeded)
                {
                    ViewData["Succeeded"] = result.Succeeded;
                    return RedirectToAction(nameof(Customers));
                }
            }

            return View();
        }

        [HttpPatch]
        [Authorize(Roles = "Manager, Admin")]
        [Route("ativar/{customerId}")]
        public async Task<IActionResult> Activate(int customerId)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();
                Response<UpdateCustomerDto> result = await _customerService.ActivateCustomerAsync(customerId, authenticatedUser.CompanyId);

                if (result.Succeeded)
                {
                    return Json(new { success = true, message = result.Message });
                }

                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = false, message = "Falha ao ativar cliente." });
        }

        [HttpPatch]
        [Authorize(Roles = "Manager, Admin")]
        [Route("desativar/{customerId}")]
        public async Task<IActionResult> Inactivate(int customerId)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();
                Response<UpdateCustomerDto> result = await _customerService.InactivateCustomerAsync(customerId, authenticatedUser.CompanyId);

                if (result.Succeeded)
                {
                    return Json(new { success = true, message = result.Message });
                }

                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = false, message = "Falha ao desativar cliente." });
        }

        [HttpDelete]
        [Authorize(Roles = "Manager, Admin")]
        [Route("deletar/{customerId}")]
        public async Task<IActionResult> DeleteCustomer(int customerId)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();
                Response<GetCustomerDto> result = await _customerService.DeleteCustomerAsync(customerId, authenticatedUser.CompanyId);

                if (result.Succeeded)
                {
                    return Json(new { success = true, message = result.Message });
                }

                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = false, message = "Falha ao deletar cliente." });
        }
    }
}