using System.ComponentModel.DataAnnotations;

namespace ShelterMaker.DTOs
{
    public class ContactMaintenanceCreateDTO
    {
        [Required]
        public string Type { get; set; }
    }
    public class ContactMaintenanceDetailDTO 
    { 
        public int id { get; set; }
        public string Type { get; set; }
    }
}
