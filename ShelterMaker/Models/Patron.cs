using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class Patron
{
    public int PatronId { get; set; }

    public int? PersonId { get; set; }

    public int? IntakeId { get; set; }

    public int? BedId { get; set; }

    public DateTime? LastCheckIn { get; set; }

    public int? WorkPassId { get; set; }

    public string? PassPhrase { get; set; }

    public bool? IsActive { get; set; }

    public int? FacilityId { get; set; }

    public virtual Bed? Bed { get; set; }

    public virtual ICollection<CaseWorkerPatron> CaseWorkerPatrons { get; set; } = new List<CaseWorkerPatron>();

    public virtual ICollection<EmergencyContact> EmergencyContacts { get; set; } = new List<EmergencyContact>();

    public virtual Facility? Facility { get; set; }

    public virtual ICollection<InfoRelease> InfoReleases { get; set; } = new List<InfoRelease>();

    public virtual Intake? Intake { get; set; }

    public virtual ICollection<MedicalCondition> MedicalConditions { get; set; } = new List<MedicalCondition>();

    public virtual ICollection<PatronBan> PatronBans { get; set; } = new List<PatronBan>();

    public virtual ICollection<PatronIncident> PatronIncidents { get; set; } = new List<PatronIncident>();

    public virtual ICollection<PatronInfoRelease> PatronInfoReleases { get; set; } = new List<PatronInfoRelease>();

    public virtual ICollection<PatronNote> PatronNotes { get; set; } = new List<PatronNote>();

    public virtual Person? Person { get; set; }

    public virtual WorkPass? WorkPass { get; set; }
}
