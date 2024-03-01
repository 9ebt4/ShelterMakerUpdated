using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class PatronIncident
{
    public int PatronIncidentId { get; set; }

    public int? PatronId { get; set; }

    public int? IncidentId { get; set; }

    public virtual Incident? Incident { get; set; }

    public virtual Patron? Patron { get; set; }
}
