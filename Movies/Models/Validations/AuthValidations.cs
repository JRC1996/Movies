using FluentValidation;
using Movies.Models.ViewModels;

namespace Movies.Models.Validations
{
    public class AuthValidations : AbstractValidator<AuthViewModel>
    {
        public AuthValidations()
        {
            RuleFor(m => m.Email)
                .NotEmpty().WithMessage("This field is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(m => m.Password).NotEmpty()
                .WithMessage("This field is required.");
                
               
        }
    }
}
