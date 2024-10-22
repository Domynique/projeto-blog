using Blog.Business.Models;
using FluentValidation;

namespace Blog.Business.Base.Validations
{
    public class ComentarioValidation : AbstractValidator<Comentario>
    {
        public ComentarioValidation()
        {
            RuleFor(comentario => comentario.Conteudo)
                    .NotEmpty().WithMessage("O conteúdo é obrigatório.")
                    .Length(2, 1000).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(comentario => comentario.AutorId)
                    .NotEmpty().WithMessage("O autor é obrigatório.");
            
        }
    }
}
