using FluentValidation;
using Movies.Models.ViewModels;

namespace Movies.Models.Validations
{
    public class GenreValidations : AbstractValidator<GenreViewModel>
    {
        public GenreValidations()
        {
            RuleFor(m => m.GenreName)
                .NotEmpty().WithMessage("This field is required.").WithName("Genre")
                .MaximumLength(50).WithMessage("The genre must be less than 50 characters long.")
                .MinimumLength(1).WithMessage("The genre must be at least 1 character long.");
            RuleFor(m => m.Description)
                .NotEmpty().WithMessage("This field is required.")
                .MaximumLength(500).WithMessage("The description must be less than 500 characters long.");
        }
    }
    
}
