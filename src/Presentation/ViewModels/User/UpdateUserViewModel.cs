using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels.User
{
    public class UpdateUserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Seu nome é obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Seu nome de usuário é obrigatório")]
        public string Username { get; set; }
        
        [Required(ErrorMessage = "Seu e-mail é obrigatório")]
        public string Email { get; set; }  
    }
}