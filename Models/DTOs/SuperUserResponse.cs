namespace CriticalConditionBackend.Models.DTOs
{
    public class SuperUserResponse
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string HospitalName { get; set; }
        public string HospitalSpeciality { get; set; }
        public string HospitalCountry { get; set; }
        public string HospitalState { get; set; }
        public string HospitalCity { get; set; }
        public string HospitalStreet { get; set; }
        public bool IsSubscriber { get; set; }
        public bool IsDataShareProgramMember { get; set; }
        public int ExcludedDevicesNumber { get; set; }
    }
}
