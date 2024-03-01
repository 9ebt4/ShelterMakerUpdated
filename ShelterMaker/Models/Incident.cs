using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class Incident
{
    public int IncidentId { get; set; }

    public int? AssociateId { get; set; }

    public DateTime? DateCreated { get; set; }

    public string? Content { get; set; }

    public DateTime? IncidentDate { get; set; }

    public int? IncidentMaintenanceId { get; set; }

    public string? ActionTaken { get; set; }

    public bool? EmergencyServices { get; set; }

    public virtual Associate? Associate { get; set; }

    public virtual ICollection<Ban> Bans { get; set; } = new List<Ban>();

    public virtual IncidentMaintenance? IncidentMaintenance { get; set; }

    public virtual ICollection<PatronIncident> PatronIncidents { get; set; } = new List<PatronIncident>();
}
