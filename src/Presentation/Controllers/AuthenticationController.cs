using Application.Contracts.Services;
using Application.DTOs.Authentication;
using Application.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Common;
using Presentation.ViewModels.Authentication;

namespace Presentation.Controllers
{
    public class AuthenticationController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpGet]
        [Route("login/")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login/")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserViewModel loginUserViewModel)
        {
            if (ModelState.IsValid)
            {
                LoginUserDto loginUserDto = new()
                {
                    UserName = loginUserViewModel.UserName,
                    Password = loginUserViewModel.Password
                };

                Response<string> result = await _authenticationService.LoginAsync(loginUserDto);
                ViewData["Message"] = result.Message;

                if (result.Succeeded)
                {
                    return RedirectToAction("home", "Home");
                }
            }

            return View();
        }

        [HttpGet("recuperar-senha/")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost("recuperar-senha/")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Response<string> result = await _authenticationService.ForgotPasswordAsync(viewModel.Email, Request.Headers.Origin.ToString());
                ViewData["Message"] = result.Message;

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }
            }

            return View();
        }

        [HttpGet("recuperar-senha/confirmar")]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet("nova-senha/")]
        public IActionResult ResetPassword()
        {
            string token = Request.Query["token"];
            string email = Request.Query["email"];

            // armazenar os valores em cookies
            Response.Cookies.Append("ResetPasswordToken", token, new CookieOptions { HttpOnly = true, Secure = true });
            Response.Cookies.Append("ResetPasswordEmail", email, new CookieOptions { HttpOnly = true, Secure = true });

            return View();
        }

        [HttpPost("nova-senha/")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // recuperar os valores dos cookies
                string token = Request.Cookies["ResetPasswordToken"].ToString();
                string email = Request.Cookies["ResetPasswordEmail"].ToString();

                ResetPasswordDto dto = ResetPasswordViewModel.MapToDto(viewModel, token, email);
                var result = await _authenticationService.ResetPasswordAsync(dto);

                ViewData["Succeeded"] = result.Succeeded;
                ViewData["Message"] = result.Message;

                if (result.Succeeded)
                {
                    // remover os cookies após a requisição bem sucedida
                    Response.Cookies.Delete("ResetPasswordToken");
                    Response.Cookies.Delete("ResetPasswordEmail");

                    return RedirectToAction(nameof(Login));
                }

                return View();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authenticationService.LogoutAsync();
            SessionService.RemoveUserSession();
            return RedirectToAction(nameof(Login));
        }
    }
}