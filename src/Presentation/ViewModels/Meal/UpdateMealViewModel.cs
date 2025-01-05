using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels.Meal
{
    public class UpdateMealViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do sabor não pode ser vazio")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O sabor precisa ter no mínimo 3 caracteres e no máximo 50.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Os acompanhamentos não podem ficar em branco")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Os acompanhamentos precisam ter no mínimo 3 caracteres e no máximo 100.")]
        public string Accompaniments { get; set; }

        public int UserCompanyId { get; set; }

        public DetailMealViewModel Meal { get; set; }
    }
}