using System.ComponentModel.DataAnnotations;

namespace Movies.Models.ViewModels
{
    public class UserViewModel
    {
        [Key]
        public int UserId{ get; set; }
     
        public string FullName { get; set; } = null!;
       
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string ConfirmPassword { get; set; } = null!;

    }
}
