using System.ComponentModel.DataAnnotations;

namespace CriticalConditionBackend.Models.DTOs
{
    public class SubUserLogin
    {
        [Required]
        public string Code { get; set; }
    }
}
