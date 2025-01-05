using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Meal
{
    public class CreateMealDto
    {
        [Required(ErrorMessage = "A descrição do sabor é obrigatória.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "A descrição deve conter entre 2 e 100 caracteres.")]
        public string Description { get; set; }
        public string Accompaniments { get; set; }
        public string UserId { get; set; }
        public int CompanyId { get; set; }
    }
}