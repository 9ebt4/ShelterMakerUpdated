using System.ComponentModel.DataAnnotations;

namespace ShelterMaker.DTOs
{
    
        public class BanCreateDto
        {
            [Required]
            public DateTime StartDate { get; set; } // Now required by annotation

            public DateTime? EndDate { get; set; } // Still optional

            [Required]
            public int IncidentReportId { get; set; } // Required by annotation

            [Required]
            public int BanMaintenanceId { get; set; } // Required by annotation

            [Required]
            [MinLength(1, ErrorMessage = "At least one PatronId is required.")]
            public List<int> PatronIds { get; set; } = new List<int>(); // Ensure at least one ID is provided
        }

        public class BanUpdateDto
        {
            public int? IncidentReportId { get; set; }
            public bool? IsActive { get; set; }
            public int? BanMaintenanceId { get; set; }
        }
    public class BanDetailDto
    {
        public int? Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
        public int? IncidentReportID { get; set; }
        public int? BanMaintenanceId { get; set; }
        public string? Category { get; set; }
    }
}
