namespace ShelterMaker.DTOs
{
    public class NoteCreateDto
    {
        public int AssociateId { get; set; }
        public List<int> PatronIds { get; set; } = new List<int>();
        public string Content { get; set; }
        public int NoteMaintenanceId { get; set; }
    }

    public class NoteDetailsDto
    {
        public int NoteId { get; set; }
        public int? AssociateId { get; set; }
        public string AssociateName { get; set; }
        public string Content { get; set; }
        public DateTime? DateCreated { get; set; }
        public string NoteMaintenanceCategory { get; set; }
        public List<int?> PatronIds { get; set; }
        public List<string> PatronNames { get; set; }

        public NoteDetailsDto()
        {
            PatronIds = new List<int?>();
            PatronNames = new List<string>();
        }
    }

    public class UpdateNoteDto
    {
        public string? Content { get; set; }
        public int? NoteMaintenanceId { get; set; }
    }


}
