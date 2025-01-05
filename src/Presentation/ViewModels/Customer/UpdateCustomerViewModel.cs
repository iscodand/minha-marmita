using System.ComponentModel.DataAnnotations;
using Application.Dtos.Customer;

namespace Presentation.ViewModels.Customer
{
    public class UpdateCustomerViewModel
    {
        public int Id { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Seu nome precisa ter no mínimo 3 caracteres e no máximo 50.")]
        public string Name { get; set; }

        [StringLength(16, MinimumLength = 16, ErrorMessage = "O número de telefone precisa ter exatamente os 11 dígitos.")]
        public string Phone { get; set; }

        public GetCustomerDto DetailCustomer { get; set; }
    }
}