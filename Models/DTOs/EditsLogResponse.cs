using System;

namespace CriticalConditionBackend.Models.DTOs
{
    public class EditsLogResponse
    {
        public string SubUserUserName { get; set; }
        public string Action { get; set; }
        public string DeviceName { get; set; }
    }
}
