using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class IncidentMaintenance
{
    public int IncidentMaintenanceId { get; set; }

    public string? Category { get; set; }

    public virtual ICollection<Incident> Incidents { get; set; } = new List<Incident>();
}
