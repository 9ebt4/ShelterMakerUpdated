namespace ShelterMaker.DTOs
{
    public class IncidentCreateDto
    {
        public int AssociateId { get; set; }
        public DateTime IncidentDate { get; set; }
        public int IncidentMaintenanceId { get; set; }
        public string ActionTaken { get; set; }
        public bool EmergencyServices { get; set; }
        public List<int> PatronIds { get; set; } = new List<int>(); // IDs of patrons involved in the incident
    }

    public class IncidentDetailDto
    {
        public int IncidentId { get; set; }
        public DateTime? DateCreated { get; set; }
        public string Content { get; set; }
        public DateTime? IncidentDate { get; set; }
        public string ActionTaken { get; set; }
        public bool? EmergencyServices { get; set; }
        public AssociateDetailDto AssociateDetails { get; set; }
        public List<PatronDetailDto> PatronDetails { get; set; } = new List<PatronDetailDto>();
        public string IncidentCategory { get; set; }
    }

    public class AssociateDetailDto
    {
        public int AssociateId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class PatronDetailDto
    {
        public int PatronId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class IncidentUpdateDto
    {
        public string? Content { get; set; }
        public DateTime? IncidentDate { get; set; }
        public int? IncidentMaintenanceId { get; set; }
        public string? ActionTaken { get; set; }
        public bool? EmergencyServices { get; set; }
    }
}
