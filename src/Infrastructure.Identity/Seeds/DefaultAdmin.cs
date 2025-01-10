using Application.Contracts.Repositories;
using Application.Contracts.Services;
using Application.DTOs.Authentication;
using Application.DTOs.Company.Requests;
using Application.Wrappers;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Seeds
{
    public static class DefaultAdmin
    {
        public static async Task SeedAsync(UserManager<User> userManager, ICompanyService companyService)
        {
            RegisterUserDto registerUserDto = new()
            {
                Name = "Isco D'Andrade",
                Username = "iscodand",
                Email = "iscodand@email.com",
                Password = "pass123!",
                ConfirmPassword = "pass123!",
                CompanyId = 1,
                Role = nameof(RoleEnum.SuperAdmin)
            };

            CreateCompanyRequest createCompany = new()
            {
                Name = "Delícias da Soraya",
                CNPJ = "59.287.230/0001-26"
            };

            Response<string> createCompanyResult = await companyService.CreateCompanyAsync(createCompany);

            bool userAlreadyRegistered = userManager.Users.Any(x => x.UserName == registerUserDto.Username);
            if (userAlreadyRegistered == false && createCompanyResult.Succeeded)
            {
                User user = User.Create(registerUserDto.Name,
                            registerUserDto.Email,
                            registerUserDto.Username,
                            int.Parse(createCompanyResult.Data));

                await userManager.CreateAsync(user, registerUserDto.Password);
                await userManager.AddToRoleAsync(user, registerUserDto.Role);
            }
        }
    }
}