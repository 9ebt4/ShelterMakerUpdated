using System.ComponentModel.DataAnnotations;

namespace ShelterMaker.DTOs
{
    public class MedicalConditionCreateDto
    {
        public int PatronId { get; set; }

        [Required]
        public string Details { get; set; }

        public int MedicalConditionMaintenanceId { get; set; }
    }

    public class MedicalConditionDetailsDto
    {
        public int? MedicalConditionId { get; set; }
        public int? PatronId { get; set; }
        public string? Details { get; set; }
        public int? MedicalConditionMaintenanceId { get; set; }
        public string? MedicalConditionMaintenanceCategory { get; set; }
    }

    public class MedicalConditionUpdateDto
    {
        public int? MedicalConditionMaintenanceId { get; set; }
        public string? Details { get; set; }
    }
}
