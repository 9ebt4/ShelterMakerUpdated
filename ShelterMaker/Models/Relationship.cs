using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class Relationship
{
    public int RelationshipId { get; set; }

    public string? Relationship1 { get; set; }

    public virtual ICollection<EmergencyContact> EmergencyContacts { get; set; } = new List<EmergencyContact>();

    public virtual ICollection<InfoRelease> InfoReleases { get; set; } = new List<InfoRelease>();

    public virtual ICollection<PatronInfoRelease> PatronInfoReleases { get; set; } = new List<PatronInfoRelease>();
}
