using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class Person
{
    public int PersonId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? BirthDay { get; set; }

    public int? GenderId { get; set; }

    public string? MiddleName { get; set; }

    public virtual ICollection<CaseWorkerPatron> CaseWorkerPatrons { get; set; } = new List<CaseWorkerPatron>();

    public virtual ICollection<ContactInfo> ContactInfos { get; set; } = new List<ContactInfo>();

    public virtual ICollection<EmergencyContact> EmergencyContacts { get; set; } = new List<EmergencyContact>();

    public virtual Gender? Gender { get; set; }

    public virtual ICollection<GoogleUser> GoogleUsers { get; set; } = new List<GoogleUser>();

    public virtual ICollection<PatronInfoRelease> PatronInfoReleases { get; set; } = new List<PatronInfoRelease>();

    public virtual ICollection<Patron> Patrons { get; set; } = new List<Patron>();
}
