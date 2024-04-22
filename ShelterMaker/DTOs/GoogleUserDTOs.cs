namespace ShelterMaker.DTOs
{
    public class GoogleUserCreateDto
    {
        public string GoogleToken { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FacilityPasscode { get; set; } = null!;
    }

    public class GoogleUserUpdateDto
    {
        public string? GoogleToken { get; set; }
        public bool? IsActive { get; set; }
    }
    public class GoogleUserDetailDTO
    {
        public int googleId { get; set; }
        public string googleToken { get; set;}
        public int? facilityId { get; set;}
        public bool? facilityIsActive { get; set; }
        public int? associateId { get; set; }
        public bool? associateIsActive { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }

    }
}
