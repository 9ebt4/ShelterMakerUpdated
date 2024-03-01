using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class Ban
{
    public int BanId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool? IsActive { get; set; }

    public int? IncidentReportId { get; set; }

    public int? BanMaintenanceId { get; set; }

    public virtual BanMaintenance? BanMaintenance { get; set; }

    public virtual Incident? IncidentReport { get; set; }

    public virtual ICollection<PatronBan> PatronBans { get; set; } = new List<PatronBan>();
}
