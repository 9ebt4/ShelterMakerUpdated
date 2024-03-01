using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class PatronNote
{
    public int PatronNoteId { get; set; }

    public int? PatronId { get; set; }

    public int? NoteId { get; set; }

    public virtual Note? Note { get; set; }

    public virtual Patron? Patron { get; set; }
}
