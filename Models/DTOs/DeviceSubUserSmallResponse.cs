using System;

namespace CriticalConditionBackend.Models.DTOs
{
    public class DeviceSubUserSmallResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Area { get; set; }
        public int NumberOfFailures { get; set; }
        public int DownTime { get; set; }
        public bool IsIoT { get; set; }
    }
}
