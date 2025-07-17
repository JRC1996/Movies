using FluentValidation;
using Movies.Models.ViewModels;

namespace Movies.Models.Validations
{
    public class AgeRatingValidations : AbstractValidator<AgeRatingViewModel>
    {

        public AgeRatingValidations()
        {
            RuleFor(m => m.RatingName)
                .NotEmpty().WithMessage("This field is required.").WithName("Age Rating")
                .MaximumLength(50).WithMessage("The age rating must be less than 50 characters long.")
                .MinimumLength(1).WithMessage("The age rating must be at least 1 character long.");
            RuleFor(m => m.Description)
                .NotEmpty().WithMessage("This field is required.")
                .MaximumLength(500).WithMessage("The description must be less than 500 characters long.");
         
        }
    }
}
