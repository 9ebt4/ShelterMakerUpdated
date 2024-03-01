using System.ComponentModel.DataAnnotations;

namespace ShelterMaker.DTOs
{
    public class CaseWorkerPatronDTO
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
    }
}
