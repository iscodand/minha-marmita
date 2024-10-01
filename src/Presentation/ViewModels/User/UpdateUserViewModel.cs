using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels.User
{
    public class UpdateUserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Seu nome � obrigat�rio")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Seu nome de usu�rio � obrigat�rio")]
        public string Username { get; set; }
        
        [Required(ErrorMessage = "Seu e-mail � obrigat�rio")]
        public string Email { get; set; }  
    }
}