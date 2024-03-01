using System.ComponentModel.DataAnnotations;

namespace ShelterMaker.DTOs
{
    public class FacilityDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Facility Code cannot exceed 20 characters")]
        public string FacilityCode { get; set; }

    }

    public class FacilityUpdateDto
    {
        [MaxLength(100)]
        public string? Name { get; set; }
        [MaxLength(20)]
        public string? FacilityCode { get; set; }
        public bool? IsActive { get; set; }
    }
}
