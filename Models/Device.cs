using System;

namespace CriticalConditionBackend.Models
{
    public class Device
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string TypeOfService { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int NumberOfWorkingDays { get; set; }
        public int NumberOfFailures { get; set; }
        public int DownTime { get; set; }
        public string Safety { get; set; }
        public string Function { get; set; }
        public string Area { get; set; }
        public double ServiceCost { get; set; }
        public double OperationCost { get; set; }
        public double PurchasingCost { get; set; }
        public string Detection { get; set; }
        public int FMEARiskScore { get; set; }
        public bool IsExcludedFromDSP { get; set; }
        public bool IsArchived { get; set; }
        public bool IsIoT { get; set; }
        public int? SecurityRiskScore { get; set; }
        public string AddedBy { get; set; }
        public string LastEditBy { get; set; }
        public DateTime AddedAt { get; set; }
        public DateTime LastEditDate { get; set; }
        public string SuperUserEmail { get; set; }
    }
}
