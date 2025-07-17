using FluentValidation;
using Movies.Models.ViewModels;

namespace Movies.Models.Validations
{
    public class MoviesValidations : AbstractValidator<MovieViewModel>
    {

        public MoviesValidations()
        {
            RuleFor(m => m.Name).NotEmpty().WithMessage("This field is Required")
            .NotEmpty().Length(1, 100).WithMessage("The name must be between 1 and 100 characters long.");
            RuleFor(m => m.IdGenre).NotEmpty().WithMessage("This field is Required");
            RuleFor(m => m.IdAgeRating).NotEmpty().WithMessage("This field is Required");
            RuleFor(m => m.ImageURL).MaximumLength(500).WithMessage("The image URL must be less than 500 characters long.");
            RuleFor(m => m.DurationMinutes).GreaterThanOrEqualTo(1).WithMessage("The minimum value must be 1.");
            RuleFor(m => m.Resume).MaximumLength(500).WithMessage("The resume must be less than 500 characters long.");
        }
    }
}
