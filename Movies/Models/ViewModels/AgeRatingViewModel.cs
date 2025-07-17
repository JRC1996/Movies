using System.ComponentModel.DataAnnotations;

namespace Movies.Models.ViewModels
{
    public class AgeRatingViewModel
    {
        [Key]
        public int IdAgeRaing { get; set; }

        public string RatingName { get; set; }
       
        public string Description { get; set; }
      
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
