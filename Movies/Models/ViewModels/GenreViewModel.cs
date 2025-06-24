using System.ComponentModel.DataAnnotations;

namespace Movies.Models.ViewModels
{
    public class GenreViewModel
    {
        [Key]
        public int IdGenre { get; set; }

        [Required]
        [Display(Name = "Genre")]
        [StringLength(50, ErrorMessage = "This field must be between 1 and 50 characters long.", MinimumLength = 1)]
        public string GenreName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
