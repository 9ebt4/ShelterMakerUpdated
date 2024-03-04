namespace ShelterMaker.DTOs
{
    public class EmergencyContactCreateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RelationshipId { get; set; }
        public string ContactDetail { get; set; } // Phone number or email
        public int ContactMaintenanceId { get; set; } // Defines the type of contact (phone, email, etc.)
        public int? AssociateId { get; set; }
        public int? PatronId { get; set; }
    }


    public class EmergencyContactDetailsDto
    {
        public int EmergencyContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Relationship { get; set; }
        public List<ContactInfoDto> ContactDetails { get; set; } = new List<ContactInfoDto>();
        public int? AssociateId { get; set; }
        public int? PatronId { get; set; }
    }

    public class ContactInfoDto
    {
        public string Detail { get; set; }
        public string Type { get; set; } // Type from ContactMaintenance (e.g., Phone, Email)
    }

    public class EmergencyContactSimpleUpdateDto
    { 
        public int? RelationshipId { get; set; }
    }
}
