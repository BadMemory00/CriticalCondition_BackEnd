using System;
using System.ComponentModel.DataAnnotations;
using CriticalConditionBackend.Validations;

namespace CriticalConditionBackend.Models.DTOs
{
    public class DeviceCreationRequest
    {
        [Required(ErrorMessage = "Device Name is Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Device Brand is Required")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Device Model is Required")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Device Type Of Service is Required")]
        public string TypeOfService { get; set; }

        [Required(ErrorMessage = "Device Purchase Date is Required")]
        public DateTime PurchaseDate { get; set; }

        [Required(ErrorMessage = "Device Number Of Working Days is Required")]
        [Range(0, 365, ErrorMessage = "Number Of Working Days must have a positive value under 365")]
        public int NumberOfWorkingDays { get; set; }

        [Required(ErrorMessage = "Device Number Of Failures is Required")]
        [Range(0, int.MaxValue, ErrorMessage = "Number Of Failures is outside the allowed value range")]
        public int NumberOfFailures { get; set; }

        [Required(ErrorMessage = "Device Down Time is Required")]
        [Range(0, 365, ErrorMessage = "Down Time must have a positive value under 365")]
        public int DownTime { get; set; }

        [Required(ErrorMessage = "Device Safety is Required")]
        [StringRange(AllowableValues = new string[] { "Catastrophic", "Critical", "Serious", "Minor", "Negligible" })]
        public string Safety { get; set; }

        [Required(ErrorMessage = "Device Function is Required")]
        [StringRange(AllowableValues = new string[] { "LifeSupport", "Diagnostic", "TherapyAnalysis", "Monitoring", "Micekkes" })]
        public string Function { get; set; }

        [Required(ErrorMessage = "Device Area is Required")]
        [StringRange(AllowableValues = new string[] { "OR", "EmergencyICU", "RadiologyLabs", "InternalUnits", "Other" })]
        public string Area { get; set; }

        [Required(ErrorMessage = "Device Service Cost is Required")]
        [Range(0, double.MaxValue, ErrorMessage = "Service Cost is outside the allowed value range")]
        public double ServiceCost { get; set; }

        [Required(ErrorMessage = "Device Operation Cost is Required")]
        [Range(0, double.MaxValue, ErrorMessage = "Operation Cost is outside the allowed value range")]
        public double OperationCost { get; set; }

        [Required(ErrorMessage = "Device Purchasing Cost is Required")]
        [Range(0, double.MaxValue, ErrorMessage = "Purchasing Cost is outside the allowed value range")]
        public double PurchasingCost { get; set; }

        [Required(ErrorMessage = "Device Detection is Required")]
        [StringRange(AllowableValues = new string[] { "NoChance", "LowChance", "ModerateChance", "HighChance", "EasyToDetect" })]
        public string Detection { get; set; }
    }
}
