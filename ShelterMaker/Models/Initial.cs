using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class Initial
{
    public int InitialId { get; set; }

    public bool? Locations { get; set; }

    public bool? Medical { get; set; }

    public bool? Covid { get; set; }

    public bool? InitialAgreement { get; set; }

    public virtual ICollection<Intake> Intakes { get; set; } = new List<Intake>();
}
