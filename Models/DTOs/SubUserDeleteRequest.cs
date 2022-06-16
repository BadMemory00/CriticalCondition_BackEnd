using System.ComponentModel.DataAnnotations;

namespace CriticalConditionBackend.Models.DTOs
{
    public class SubUserDeleteRequest
    {
        [Required(ErrorMessage = "SubUser Code Cannot be Empty")]
        public string Code { get; set; }
    }
}
