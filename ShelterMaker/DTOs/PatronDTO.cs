namespace ShelterMaker.DTOs
{
    public class PatronCreateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public bool IsSexualOffender { get; set; }
        public int FacilityId { get; set; }
    }


    public class PatronDetailDto
    {
        public int PatronId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Age { get; set; } // Assuming this is calculated
        public string? Gender { get; set; }
        public string? PassPhrase { get; set; }
        public BedNameDTO? Bed { get; set; } // Null if no bed is assigned
        public WorkPassDto? WorkPass { get; set; }
        public InitialCheckDto? Initial { get; set; }
        public RequirementsDto? Requirements { get; set; }
        public SexualOffenderDto? SexOffender { get; set; }
        public TenRuleDetailDto? TenRules { get; set; }
        public List<MedicalConditionDetailsDto>? MedicalConditions { get; set; }
        public List<BanDetailDto>? BanDetails { get; set; }
        public List<ContactInfoDto>? ContactInfos { get; set; }
        public List<EmergencyContactDetailsDto>? EmergencyContacts { get; set; }
        public List<CaseWorkerInfoDto>? CaseWorkers { get; set; }
        public List<InfoReleaseDto>? InfoReleases { get; set; }
       
    }
    public class PatronQueryParameters
    {
        public DateTime? CheckInStart { get; set; }
        public DateTime? CheckInEnd { get; set; }
        public string? Name { get; set; }
        public bool? RequireComplete { get; set; }
    }

    public class PatronListDto
    {
        public int PatronId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? LastCheckIn { get; set; }
        public BedNameDTO? Bed { get; set; }
        public WorkPassDto? WorkPass { get; set; }
        public InitialCheckDto? Initial { get; set; }
        public RequirementsDto? Requirements { get; set; }
        public SexualOffenderDto? SexOffender { get; set; }
        public TenRuleDetailDto? TenRules { get; set; }
    }

    public class PatronUpdateDto
    {
        public DateTime? LastCheckIn { get; set; }
        public bool? IsActive { get; set; }
        public int? BedId { get; set; }
    }




}
