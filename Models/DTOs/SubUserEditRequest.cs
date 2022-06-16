using System.ComponentModel.DataAnnotations;

namespace CriticalConditionBackend.Models.DTOs
{
    public class SubUserEditRequest
    {
        [Required(ErrorMessage = "SubUser Code Cannot be Empty")]
        public string Code { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
    }
}
