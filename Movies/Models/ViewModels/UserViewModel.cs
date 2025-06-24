using System.ComponentModel.DataAnnotations;

namespace Movies.Models.ViewModels
{
    public class UserViewModel
    {

        public int IdUser { get; set; }
        [Required]
        public string FullName { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;

        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    }
}
