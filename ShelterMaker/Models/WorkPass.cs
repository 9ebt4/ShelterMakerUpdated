using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class WorkPass
{
    public int WorkPassId { get; set; }

    public bool? Needed { get; set; }

    public bool? Confirmed { get; set; }

    public virtual ICollection<Patron> Patrons { get; set; } = new List<Patron>();
}
