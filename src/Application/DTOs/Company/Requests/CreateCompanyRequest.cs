using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Company.Requests
{
    public class CreateCompanyRequest
    {
        [Required(ErrorMessage = "Nome da empresa é obrigatório.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "CNPJ da empresa é obrigatório.")]
        [StringLength(18, ErrorMessage = "Você precisa inserir 18 caracteres.")]
        public string CNPJ { get; set; }

        public static Domain.Entities.Company Map(Domain.Entities.Company company, CreateCompanyRequest request)
        {
            company.Name = request.Name;
            company.CNPJ = request.CNPJ;

            return company;
        }
    }
}