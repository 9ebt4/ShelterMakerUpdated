using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class Intake
{
    public int IntakeId { get; set; }

    public int? InitialId { get; set; }

    public int? SexualOffenderId { get; set; }

    public int? RequirementsId { get; set; }

    public int? TenRulesId { get; set; }

    public virtual Initial? Initial { get; set; }

    public virtual ICollection<Patron> Patrons { get; set; } = new List<Patron>();

    public virtual Requirement? Requirements { get; set; }

    public virtual SexualOffender? SexualOffender { get; set; }

    public virtual TenRule? TenRules { get; set; }
}
