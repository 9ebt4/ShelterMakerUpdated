namespace ShelterMaker.DTOs
{
    public class AssociateCreateDto
    {
        public int AssociateMaintenanceId { get; set; }
        public int GoogleUserId { get; set; }

    }
    public class AssociateStatusUpdateDto
    {
        public bool IsActive { get; set; }
    }
    public class AssociateMaintenanceUpdateDto
    {
        public int AssociateMaintenanceId { get; set; }
    }
}
