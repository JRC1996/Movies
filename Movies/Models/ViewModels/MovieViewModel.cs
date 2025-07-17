using System.ComponentModel.DataAnnotations;

namespace Movies.Models.ViewModels
{
    public class MovieViewModel
    {
     
        [Key]
        public int IdMovie { get; set; }

        public string Name { get; set; }

        public int IdGenre { get; set; }

        public string? Genre { get; set; }

        public int IdAgeRating { get; set; }

        public string? AgeRating { get; set; }
        public string? ImageURL { get; set; }

        public int? DurationMinutes { get; set; }

        public string? Resume { get; set; }
        public DateOnly? ReleaseDate { get; set; }
    }
}
