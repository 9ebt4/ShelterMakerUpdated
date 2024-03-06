namespace ShelterMaker.DTOs
{
    public class GoogleUserCreateDto
    {
        public string GoogleToken { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }

    public class GoogleUserUpdateDto
    {
        public string? GoogleToken { get; set; }
        public bool? IsActive { get; set; }
    }
}
