using System.ComponentModel.DataAnnotations;

namespace CriticalConditionBackend.Models.DTOs
{
    public class DeviceArchiveAndUnarchiveAndDeleteRequest
    {
        [Required(ErrorMessage = "Device ID cannot be empty")]
        public Guid DeviceId { get; set; }
    }
}
