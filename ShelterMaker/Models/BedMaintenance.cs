using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class BedMaintenance
{
    public int BedMaintenanceId { get; set; }

    public int? BedId { get; set; }

    public string? Category { get; set; }

    public virtual Bed? Bed { get; set; }
}
