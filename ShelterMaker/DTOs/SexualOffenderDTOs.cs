namespace ShelterMaker.DTOs
{
    public class SexualOffenderCreateDto
    {
        public bool Completed { get; set; } = false;
        public bool IsOffender { get; set; } = false;
    }

    public class SexualOffenderUpdateDto
    {
        public bool? Completed { get; set; }
        public bool? IsOffender { get; set; }
    }

    public class SexualOffenderDto
    {
        public int SexualOffenderId { get; set; }
        public bool? Completed { get; set; }
        public bool? IsOffender { get; set; }
    }

}
