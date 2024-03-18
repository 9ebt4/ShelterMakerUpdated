using System.ComponentModel.DataAnnotations;

namespace ShelterMaker.DTOs
{
    
        public class CaseWorkerPatronCreateDto
        {
            [Required]
            public int PatronId { get; set; }
            [Required]
            public int CaseWorkerId { get; set; }
        }

        public class UpdateAssociationDto
        {
            public int? NewCaseWorkerId { get; set; }
            public int? NewPatronId { get; set; }
        }

    public class CaseWorkerInfoDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<ContactInfoDto> ContactDetails { get; set; } = new List<ContactInfoDto>();

    }

}
