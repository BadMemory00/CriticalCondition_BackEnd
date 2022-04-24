using System.ComponentModel.DataAnnotations;
using CriticalConditionBackend.Validations;

namespace CriticalConditionBackend.Models.DTOs
{
    public class SubUserGeneration
    {
        [Required(ErrorMessage = "Sub-user Name is Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Sub-user Role is Required")]
        [StringRange(AllowableValues = new[] { "Operator", "Technician" })]
        public string Role { get; set; }

    }
}
