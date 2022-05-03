using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CriticalConditionBackend.Models.DTOs
{
    public class SuperUserRegister
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(127, MinimumLength = 8, ErrorMessage = "Password can't be less that 8 characters long")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Phone Number is required")]
        [Phone(ErrorMessage = "Phone Number is not in Correct Format")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Hospital Name is required")]
        public string HospitalName { get; set; }
        [Required(ErrorMessage = "Hospital Speciality is required")]
        public string HospitalSpeciality { get; set; }
        [Required(ErrorMessage = "Country is required")]
        public string HospitalCountry { get; set; }
        [Required(ErrorMessage = "State is required")]
        public string HospitalState { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string HospitalCity { get; set; }
        [Required(ErrorMessage = "Street is required")]
        public string HospitalStreet { get; set; }
    }
}
