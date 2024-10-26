using Blog.Core.Models;
using FluentValidation;

namespace Blog.Core.Base.Validations
{
    public class PostValidation : AbstractValidator<Post>
    {
        public PostValidation() 
        {
            RuleFor(post => post.Titulo)
                    .NotEmpty().WithMessage("O título é obrigatório.")
                    .Length(2, 200).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(post => post.Conteudo)
                    .NotEmpty().WithMessage("O conteúdo é obrigatório.")
                    .Length(2, 1000).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(post => post.AutorId)
                    .NotEmpty().WithMessage("O ID do autor é obrigatório.");
        }
    }
}
