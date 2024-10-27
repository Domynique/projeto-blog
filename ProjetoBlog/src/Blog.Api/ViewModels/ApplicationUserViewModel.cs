using System.ComponentModel.DataAnnotations;

namespace Blog.Api.ViewModels
{
    public class ApplicationUserViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Login")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O e-mail não é válido.")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        public Guid AutorId { get; set; }

        public AutorViewModel Autor { get; set; }
    }

}
