using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class NoteMaintenance
{
    public int NoteMaintenanceId { get; set; }

    public string? Category { get; set; }

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
}
