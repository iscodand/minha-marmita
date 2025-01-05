using System.Text.Json;
using Application.Contracts.Services;
using Application.Dtos.User;
using Application.DTOs.Authentication;
using Application.Parameters;
using Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Common;
using Presentation.ViewModels.User;

namespace Presentation.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin, Manager")]
    [Route("minha-empresa/funcionarios/")]
    public class EmployeeController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;

        public EmployeeController(
            IAuthenticationService authenticationService,
            IUserService userService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Employees(int pageNumber = 1)
        {
            RequestParameter parameter = new()
            {
                PageNumber = pageNumber,
                PageSize = 10,
            };

            var authenticatedUser = SessionService.RetrieveUserSession();
            var result = await _userService.GetUsersByCompanyPagedAsync(authenticatedUser.CompanyId, parameter);

            if (result.Succeeded)
            {
                return View(result);
            }

            return View("Home");
        }

        // auth/cadastro/
        [HttpGet]
        [Route("novo/")]
        public async Task<IActionResult> RegisterEmployee()
        {
            Response<IEnumerable<GetRolesDto>> roles = await _userService.GetRolesAsync();

            RegisterUserViewModel registerUserViewModel = new()
            {
                RolesDropdown = roles.Data
            };

            return View(registerUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("novo/")]
        public async Task<IActionResult> RegisterEmployee(RegisterUserViewModel registerUserViewModel)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();

                RegisterUserDto registerUserDto = new()
                {
                    Name = registerUserViewModel.Name,
                    Username = registerUserViewModel.Username,
                    Email = registerUserViewModel.Email,
                    Password = registerUserViewModel.Password,
                    ConfirmPassword = registerUserViewModel.ConfirmPassword,

                    Role = registerUserViewModel.RoleId,
                    CompanyId = authenticatedUser.CompanyId
                };

                Response<string> result = await _authenticationService.RegisterAsync(registerUserDto);
                ViewData["Message"] = result.Message;

                if (result.Succeeded)
                {
                    ViewData["Succeeded"] = true;
                    return RedirectToAction(nameof(Employees));
                }
            }

            Response<IEnumerable<GetRolesDto>> roles = await _userService.GetRolesAsync();
            registerUserViewModel.RolesDropdown = roles.Data;
            return View(registerUserViewModel);
        }


        [HttpGet]
        [Route("{employeeUsername}")]
        public async Task<IActionResult> UpdateEmployee(string employeeUsername)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();
                Response<DetailUserDto> result = await _userService.DetailUserAsync(employeeUsername, authenticatedUser.CompanyId);

                if (result.Succeeded)
                {
                    UpdateUserViewModel updateUserViewModel = new()
                    {
                        Id = result.Data.Id,
                        Username = employeeUsername,
                        Name = result.Data.Name,
                        Email = result.Data.Email,
                    };

                    return View(updateUserViewModel);
                }
            }

            return View(nameof(Employees));
        }

        [HttpPost]
        [Route("{employeeUsername}")]
        public async Task<IActionResult> UpdateEmployee(string employeeUsername, UpdateUserViewModel updateUserViewModel)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();

                UpdateUserDto updateUserDto = new()
                {
                    Name = updateUserViewModel.Name,
                    Username = employeeUsername,
                    Email = updateUserViewModel.Email,
                    CompanyId = authenticatedUser.CompanyId
                };

                Response<UpdateUserDto> result = await _userService.UpdateUserAsync(updateUserDto);

                ViewData["Message"] = result.Message;
                ViewData["Succeeded"] = result.Succeeded;

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Employees));
                }
            }

            return View(updateUserViewModel);
        }

        [HttpPatch]
        [Route("ativar/{employeeUsername}")]
        public async Task<IActionResult> ActivateEmployee(string employeeUsername)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();
                Response<GetUserDto> result = await _userService.ActivateUserAsync(employeeUsername,
                                                                                     authenticatedUser.CompanyId);

                if (result.Succeeded)
                {
                    return Json(new { success = true });
                }

                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = false, message = "Falha ao ativar funcionário." });
        }

        [HttpPatch]
        [Route("desativar/{employeeUsername}")]
        public async Task<IActionResult> DeactivateEmployee(string employeeUsername)
        {

            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();
                Response<GetUserDto> result = await _userService.DeactivateUserAsync(employeeUsername,
                                                                                     authenticatedUser.CompanyId);

                if (result.Succeeded)
                {
                    return Json(new { success = true });
                }

                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = false, message = "Falha ao desativar funcionário." });
        }

        [HttpGet]
        [Route("{employeeUsername}/alterar-senha")]
        public async Task<IActionResult> ChangePasswordAsync(string employeeUsername)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.GetUserByUsernameAsync(employeeUsername);
                if (result.Succeeded)
                {
                    ChangePasswordDto request = new()
                    {
                        Username = result.Data.Username,
                        OldPassword = "",
                        NewPassword = "",
                        ConfirmNewPassword = ""
                    };

                    return View(request);
                }
            }

            return RedirectToAction(nameof(Employees));
        }


        [HttpPost]
        [Route("{employeeUsername}/alterar-senha")]
        public async Task<IActionResult> ChangePasswordAsync(string employeeUsername, ChangePasswordDto request)
        {
            if (ModelState.IsValid)
            {
                var result = await _authenticationService.ChangePasswordAsync(employeeUsername, request);

                ViewData["Message"] = result.Message;
                ViewData["Succeeded"] = result.Succeeded;
            }

            request.Username = employeeUsername;
            return View(request);
        }

        [HttpDelete]
        [Route("deletar/{employeeUsername}")]
        public async Task<IActionResult> DeleteEmployee(string employeeUsername)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();
                Response<GetUserDto> result = await _userService.DeactivateUserAsync(employeeUsername,
                                                                                     authenticatedUser.CompanyId);

                if (result.Succeeded)
                {
                    return Json(new { success = true });
                }

                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = false, message = "Falha ao deletar funcionário." });
        }
    }
}