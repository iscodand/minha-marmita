using Application.Dtos.User;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace Application.DTOs.Company.Response
{
    public class DetailCompanyDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public IEnumerable<GetUserDto> Employees { get; set; }

        public static DetailCompanyDTO Map(Domain.Entities.Company company)
        {
            return new()
            {
                Id = company.Id,
                Name = company.Name,
                CNPJ = company.CNPJ,
                Employees = GetUserDto.Map(company.Users)
            };
        }
    }
}