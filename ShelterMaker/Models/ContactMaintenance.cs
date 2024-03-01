using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class ContactMaintenance
{
    public int ContactMaintenanceId { get; set; }

    public string? Type { get; set; }

    public virtual ICollection<ContactInfo> ContactInfos { get; set; } = new List<ContactInfo>();
}
