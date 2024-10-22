using Blog.Business.Models;
using FluentValidation;

namespace Blog.Business.Base.Validations
{
    public class AutorValidation : AbstractValidator<Autor>
    {
        public AutorValidation() 
        {
            RuleFor(autor => autor.Nome)
                    .NotEmpty().WithMessage("O nome é obrigatório.")
                    .Length(2, 100).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");


            RuleFor(autor => autor.Biografia)
                    .Length(2, 1000).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

        }
    }
}
