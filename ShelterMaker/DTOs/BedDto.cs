using System.ComponentModel.DataAnnotations;

namespace ShelterMaker.DTOs
{
    public class BedCreateDto
    {
        [Required]
        public int FacilityId { get; set; }

        [Required]
        public string MaintenanceType { get; set; } // 'bed' or 'waitlist'

        [Required]
        public string AmenityType { get; set; } // 'bed' or 'mat'
    }
    public class BedNameDTO
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
    }
}
