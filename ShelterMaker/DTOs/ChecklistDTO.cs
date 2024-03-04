namespace ShelterMaker.DTOs
{
    public class ChecklistCreateDto
    {
        public int FacilityId { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Options { get; set; }
    }

    public class ChecklistUpdateDto
    {
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public string? Options { get; set; }
    }
}
