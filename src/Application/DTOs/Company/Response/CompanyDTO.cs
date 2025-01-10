using Domain.Entities;

namespace Application.DTOs.Company.Response
{
    public class CompanyDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public int EmployeesCount { get; set; }

        public static IEnumerable<CompanyDTO> Map(IEnumerable<Domain.Entities.Company> companies)
        {
            return companies.Select(company => new CompanyDTO
            {
                Id = company.Id,
                Name = company.Name,
                CNPJ = company.CNPJ,
                EmployeesCount = company.Users.Count
            }).ToList();
        }
    }
}