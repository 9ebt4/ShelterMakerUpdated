using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class BanMaintenance
{
    public int BanMaintenanceId { get; set; }

    public string? Category { get; set; }

    public virtual ICollection<Ban> Bans { get; set; } = new List<Ban>();
}
