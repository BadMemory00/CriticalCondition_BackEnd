using System.ComponentModel.DataAnnotations;

namespace CriticalConditionBackend.Models.DTOs
{
    public class SuperUserLogin
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
