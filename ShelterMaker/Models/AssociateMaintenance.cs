using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class AssociateMaintenance
{
    public int AssociateMaintenanceId { get; set; }

    public string? Role { get; set; }

    public virtual ICollection<Associate> Associates { get; set; } = new List<Associate>();
}
