using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class Requirement
{
    public int RequirementsId { get; set; }

    public bool? Completed { get; set; }

    public bool? Confirmed { get; set; }

    public virtual ICollection<Intake> Intakes { get; set; } = new List<Intake>();
}
