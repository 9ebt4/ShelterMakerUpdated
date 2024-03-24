namespace ShelterMaker.DTOs
{
    public class PersonCreateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly? BirthDay { get; set; }
        public int? GenderId { get; set; }
        public string? MiddleName { get; set; }
        
    }
    public class PersonDetailDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly? BirthDay { get; set; }
        public string Gender { get; set; } // Assuming you will convert GenderId to a string representation
        public string? MiddleName { get; set; }
    }
    public class PersonUpdateDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateOnly? BirthDay { get; set; }
        public int? GenderId { get; set; }
        public string? MiddleName { get; set; }
    }
}
