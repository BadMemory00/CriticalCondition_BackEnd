using System;

namespace CriticalConditionBackend.Models
{
    public class EditsLog
    {
        public Guid Id { get; set; }
        public string Action { get; set; }
        public string DeviceName { get; set; }
        public Guid DeviceId { get; set; }
        public string SubUserUserName { get; set; }
        public string SubUserCode { get; set; }
        public string SuperUserEmail { get; set; }
    }
}
