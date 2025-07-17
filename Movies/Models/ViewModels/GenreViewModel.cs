using System.ComponentModel.DataAnnotations;

namespace Movies.Models.ViewModels
{
    public class GenreViewModel
    {
        [Key]
        public int IdGenre { get; set; }
       
        public string GenreName { get; set; }
  
        public string Description { get; set; }
        
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
