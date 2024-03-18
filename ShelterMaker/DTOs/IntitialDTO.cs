namespace ShelterMaker.DTOs
{
    public class InitialUpdateDto
    {
        public bool? Locations { get; set; }
        public bool? Medical { get; set; }
        public bool? Covid { get; set; }
        public bool? InitialAgreement { get; set; }
    }

    public class InitialCheckDto
    {
        public int Id { get; set; }
        public bool complete { get; set; }
    }

}
