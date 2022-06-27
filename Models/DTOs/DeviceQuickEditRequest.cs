using System.ComponentModel.DataAnnotations;

namespace CriticalConditionBackend.Models.DTOs
{
    public class DeviceQuickEditRequest
    {
        [Required(ErrorMessage = "Device ID cannot be empty")]
        public Guid DeviceId { get; set; }

        [Required(ErrorMessage = "Device Number Of Failures is Required")]
        [Range(0, int.MaxValue, ErrorMessage = "Number Of Failures is outside the allowed value range")]
        public int NumberOfFailures { get; set; }

        [Required(ErrorMessage = "Device Down Time is Required")]
        [Range(0, 365, ErrorMessage = "Down Time must have a positive value under 365")]
        public int DownTime { get; set; }
    }
}
