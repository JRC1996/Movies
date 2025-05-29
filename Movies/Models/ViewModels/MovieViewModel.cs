using System.ComponentModel.DataAnnotations;

namespace Movies.Models.ViewModels
{
    public class MovieViewModel
    {
        [Required]
        public int IdMovie { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(100, ErrorMessage = "The name must be between 1 and 100 characters long.", MinimumLength = 1)]
        public string Name { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int IdGenre { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int IdAgeRating { get; set; }

        public string? ImageURL { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The minimum value must be 1." )]
        public int? DurationMinutes { get; set; }

        [DataType(DataType.Text)]
        public string? Resume { get; set; }
        public DateOnly? ReleaseDate { get; set; }
    }
}
