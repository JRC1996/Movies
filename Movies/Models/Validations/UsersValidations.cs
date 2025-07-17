using FluentValidation;
using Movies.Models.ViewModels;

namespace Movies.Models.Validations
{
    public class UsersValidations : AbstractValidator<UserViewModel>
    {
        public UsersValidations()
        {
            RuleFor(m => m.FullName)
                .NotEmpty().WithMessage("This field is required.")
                .Length(1, 100).WithMessage("The Name must be between 6 and 100 characters long.");
            RuleFor(m => m.Email)
                .NotEmpty().WithMessage("This field is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            RuleFor(m => m.Password)
                .NotEmpty().WithMessage("This field is required.")
                .Length(8, 100).WithMessage("The password must be between 6 and 100 characters long.");
            RuleFor(m => m.ConfirmPassword)
                .NotEmpty().WithMessage("This field is required.")
                .Equal(m => m.Password).WithMessage("The password and confirmation password do not match.");
        }
    }
    
    
}
