using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Common;
using Application.Contracts.Services;
using Application.DTOs.Company.Requests;
using Application.DTOs.Authentication;
using Presentation.ViewModels.Company;
using Application.Parameters;

namespace Presentation.Controllers
{
    [Authorize]
    [Route("empresas/")]
    public class CompanyController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly ICompanyService _companyService;
        private readonly IDataService _dataService;

        public CompanyController(IAuthenticationService authenticationService,
                                 IUserService userService,
                                 ICompanyService companyService,
                                 IDataService dataService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _companyService = companyService;
            _dataService = dataService;
        }

        [HttpGet("minha-empresa/")]
        public async Task<IActionResult> MyCompany()
        {
            var authenticatedUser = SessionService.RetrieveUserSession();
            var result = await _companyService.GetCompanyByIdAsync(authenticatedUser.CompanyId);

            // Recuperando dados da empresa para alimentar dashboard
            DateTime today = DateTime.Today.Date;
            DateTime pastMonth = DateTime.Today.Date.AddMonths(-1);
            var getDataResult = await _dataService.GetDataAsync(authenticatedUser.CompanyId, pastMonth, today);

            CompanyViewModel viewModel = new()
            {
                Company = result.Data,
                Data = getDataResult.Data
            };

            if (result.Succeeded)
            {
                return View(viewModel);
            }

            return RedirectToAction(nameof(GetCompanies));
        }

        [HttpGet("")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetCompanies(int pageNumber = 1)
        {
            RequestParameter parameters = new()
            {
                PageNumber = pageNumber,
                PageSize = 10,
            };

            var result = await _companyService.GetCompaniesAsync(parameters);

            return View(result);
        }

        [HttpGet("{companyId}")]
        public async Task<IActionResult> Detail(int companyId)
        {
            var authenticatedUser = SessionService.RetrieveUserSession();
            var result = await _companyService.GetCompanyByIdAsync(companyId);

            // Recuperando dados da empresa para alimentar dashboard
            DateTime today = DateTime.Today.Date;
            DateTime pastMonth = DateTime.Today.Date.AddMonths(-1);
            var getDataResult = await _dataService.GetDataAsync(companyId, pastMonth, today);

            CompanyViewModel viewModel = new()
            {
                Company = result.Data,
                Data = getDataResult.Data
            };

            if (result.Succeeded)
            {
                return View(viewModel);
            }

            return RedirectToAction(nameof(GetCompanies));
        }

        [HttpGet("empresas/cadastrar")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("empresas/cadastrar")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create(CreateCompanyRequest request)
        {
            if (ModelState.IsValid)
            {
                GetAuthenticatedUserDto authenticatedUser = SessionService.RetrieveUserSession();

                var result = await _companyService.CreateCompanyAsync(request);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(GetCompanies));
                }
            }

            return View();
        }

        [HttpPost("empresas/deletar")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var result = await _companyService.DeleteAsync(id);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(GetCompanies));
                }
            }

            return Redirect($"empresas/{id}");
        }
    }
}