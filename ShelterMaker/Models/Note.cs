using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class Note
{
    public int NoteId { get; set; }

    public int? AssociateId { get; set; }

    public DateTime? DateCreated { get; set; }

    public string? Content { get; set; }

    public int? NoteMaintenanceId { get; set; }

    public virtual Associate? Associate { get; set; }

    public virtual NoteMaintenance? NoteMaintenance { get; set; }

    public virtual ICollection<PatronNote> PatronNotes { get; set; } = new List<PatronNote>();
}
