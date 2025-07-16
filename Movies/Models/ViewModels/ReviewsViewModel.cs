using System.ComponentModel.DataAnnotations;

namespace Movies.Models.ViewModels
{
    public class ReviewsViewModel
    {

        [Key]
        public int UsersMovieId { get; set; }

        [Required]
        public int UserId { get; set; }
        [Required]
        public int MovieId { get; set; }
        [Range(0, 10, ErrorMessage = "The value must be between {1} and {2}")]
        public decimal? Rating { get; set; }
        [Required]
        public int IdStatus { get; set; }

        public string? Review { get; set; }

    }
}
