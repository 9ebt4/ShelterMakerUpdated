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
        public TenRulesDto? TenRules { get; set; }
        public List<MedicalConditionDetailsDto>? MedicalConditions { get; set; }
        public List<BanDetailDto>? BanDetails { get; set; }
        public List<ContactInfoDto>? ContactInfos { get; set; }
        public List<EmergencyContactDetailsDto>? EmergencyContacts { get; set; }
        public List<CaseWorkerInfoDto>? CaseWorkers { get; set; }
        public List<InfoReleaseDto>? InfoReleases { get; set; }
       
    }

    
    

    


}
