using System;

namespace CriticalConditionBackend.Models.DTOs
{
    public class DeviceSuperUserFullResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string TypeOfService { get; set; }
        public string UtilizationRate { get; set; }
        public string Unavailability { get; set; }
        public string Safety { get; set; }
        public string Importance { get; set; }
        public string FinancialScore { get; set; }
        public string Detection { get; set; }
        public string AgeFactor { get; set; }
        public int FMEARiskScore { get; set; }
        public bool IsIoT { get; set; }
        public int? SecurityRiskScore { get; set; }
    }
}
