using AutheticationServer.Models;
using FluentValidation;
using System.Data;

namespace AutheticationServer.Validators
{
    public class UsuariosValidator : AbstractValidator<UsuarioModel>
    {
        public UsuariosValidator()
        {

            RuleFor(Rule => Rule.Email)
                .NotNull()
                .NotEmpty();
            RuleFor(Rule => Rule.Role)
                .NotNull()
                .NotEmpty();
            RuleFor(Rule => Rule.Aplication)
                .NotNull()
                .NotEmpty();
            RuleFor(Rule => Rule.Expires)
                .NotEmpty()
                .NotEmpty()
                .Matches("^[0-9]*$")
                .WithMessage("O Prazo deve ser um número inteiro");
        }
    }
}
