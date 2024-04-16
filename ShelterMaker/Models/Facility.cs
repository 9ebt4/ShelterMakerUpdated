using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class Facility
{
    public int FacilityId { get; set; }

    public string? Name { get; set; }

    public string? FacilityCode { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Bed> Beds { get; set; } = new List<Bed>();

    public virtual ICollection<Checklist> Checklists { get; set; } = new List<Checklist>();

    public virtual ICollection<ContactInfo> ContactInfos { get; set; } = new List<ContactInfo>();

    public virtual ICollection<GoogleUser> GoogleUsers { get; set; } = new List<GoogleUser>();

    public virtual ICollection<Patron> Patrons { get; set; } = new List<Patron>();
}
