using System.ComponentModel.DataAnnotations;

namespace Movies.Models.ViewModels
{
    public class AgeRatingViewModel
    {
        [Required]
        public int IdAgeRaing { get; set; }

        [Required]
        [Display(Name = "Age Rating")]
        [StringLength(50, ErrorMessage = "This field must be between 1 and 50 characters long.", MinimumLength = 1)]
        public string RatingName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
