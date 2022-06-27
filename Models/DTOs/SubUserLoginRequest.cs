using System.ComponentModel.DataAnnotations;

namespace CriticalConditionBackend.Models.DTOs
{
    public class SubUserLoginRequest
    {
        [Required]
        public string Code { get; set; }
    }
}
