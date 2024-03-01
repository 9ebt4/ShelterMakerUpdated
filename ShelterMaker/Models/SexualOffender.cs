using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class SexualOffender
{
    public int SexualOffenderId { get; set; }

    public bool? Completed { get; set; }

    public bool? IsOffender { get; set; }

    public virtual ICollection<Intake> Intakes { get; set; } = new List<Intake>();
}
