namespace ShelterMaker.DTOs
{
    public class WorkPassDto
    {
        public int? Id { get; set; }
        public bool? Needed { get; set; }
        public bool? Confirmed { get; set; }
    }

    public class WorkPassUpdateDto
    {
        public bool? Needed { get; set; } 
        public bool? Confirmed { get; set; } 
    }
    public class WorkPassCreateDto
    {
        public bool? Needed { get; set; } = false;
        public bool? Confirmed { get; set; }=false;
    }

}
