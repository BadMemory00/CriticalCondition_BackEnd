namespace CriticalConditionBackend.Models.DTOs
{
    public enum Safety
    {
        Catastrophic = 5,
        Critical = 4,
        Serious = 3,
        Minor = 2,
        Negligible = 1
    }
    public enum Function
    {
        LifeSupport = 5,
        Diagnostic = 4,
        TherapyAnalysis = 3,
        Monitoring = 2,
        Micekkes = 1
    }
    public enum Area
    {
        OR = 5,
        EmergencyICU = 4,
        RadiologyLabs = 3,
        InternalUnits = 2,
        Other = 1
    }
    public enum Detection
    {
        NoChance = 5,
        LowChance = 4,
        ModerateChance = 3,
        HighChance = 2,
        EasyToDetect = 1,
    }
}
