namespace ShelterMaker.DTOs
{
    public class PersonContactInfoCreateDto
    {
        public int ContactMaintenanceId { get; set; }
        public int PersonId { get; set; }
        public string Details { get; set; }
    }

    public class FacilityContactInfoCreateDto
    {
        public int ContactMaintenanceId { get; set; }
        public int FacilityId { get; set; }
        public string Details { get; set; }
    }

    public class ContactInfoUpdateDto
    {
        public int? ContactMaintenanceId { get; set; }
        public string Details { get; set; }
        public int? PersonId { get; set; } = null; // Nullable to allow for updates that don't include this field
        public int? FacilityId { get; set; } = null; // Same as above
    }

    public class ContactInfoDto
    {
        public int Id { get; set; }
        public string Detail { get; set; }
        public string Type { get; set; } // Type from ContactMaintenance (e.g., Phone, Email)
    }
}
