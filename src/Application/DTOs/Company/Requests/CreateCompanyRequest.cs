using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Company.Requests
{
    public class CreateCompanyRequest
    {
        [Required(ErrorMessage = "Nome da empresa � obrigat�rio.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "CNPJ da empresa � obrigat�rio.")]
        public string CNPJ { get; set; }

        public static Domain.Entities.Company Map(Domain.Entities.Company company, CreateCompanyRequest request)
        {
            company.Name = request.Name;
            company.CNPJ = request.CNPJ;

            return company;
        }
    }
}