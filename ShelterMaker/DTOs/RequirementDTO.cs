namespace ShelterMaker.DTOs
{
    public class RequirementsDto
    {
        public int? Id { get; set; }
        public bool? Completed { get; set; }
        public bool? Confirmed { get; set; }
    }

    public class RequirementsCreateDto
    {
        public bool? Completed { get; set; }
        public bool? Confirmed { get; set; }
    }
    public class RequirementUpdateDto
    {
        public bool? Completed { get; set; }
        public bool? Confirmed { get; set; }
    }

}
