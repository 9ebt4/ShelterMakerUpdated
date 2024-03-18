namespace ShelterMaker.DTOs
{
    public class InfoReleaseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly? BirthDay { get; set; }
        public List<ContactInfoDto> ContactInfos { get; set; }
    }
}
