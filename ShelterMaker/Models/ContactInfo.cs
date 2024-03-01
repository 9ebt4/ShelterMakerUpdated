using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class ContactInfo
{
    public int ContactInfoId { get; set; }

    public int? ContactMaintenanceId { get; set; }

    public int? PersonId { get; set; }

    public string? Details { get; set; }

    public int? FacilityId { get; set; }

    public virtual ContactMaintenance? ContactMaintenance { get; set; }

    public virtual Facility? Facility { get; set; }

    public virtual Person? Person { get; set; }
}
