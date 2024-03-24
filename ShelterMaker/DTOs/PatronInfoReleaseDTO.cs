namespace ShelterMaker.DTOs
{
    public class PatronInfoReleaseCreateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly Age { get; set; }
        public int RelationshipId { get; set; }
        public string ContactDetail { get; set; }
        public int ContactMaintenanceId { get; set; }
        public int PatronId { get; set; }
    }
    public class PatronInfoReleaseDetailDto
    {
        public int PatronInfoReleaseId { get; set; }
        public int? PersonID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly Birthday { get; set; }
        public int PatronId { get; set; }
        public int RelationshipId { get; set; }
        public string Relationship { get; set; }
        public List<ContactInfoDto> ContactInfos { get; set; }
    }

    

}
