using System;

namespace CriticalConditionBackend.Models.DTOs
{
    public class DeviceSuperUserSmallResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public int NumberOfFailures { get; set; }
        public int FMEARiskScore { get; set; }
        public bool IsIoT { get; set; }
        public int? SecurityRiskScore { get; set; }
    }
}
