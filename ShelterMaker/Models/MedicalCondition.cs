using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class MedicalCondition
{
    public int MedicalConditionId { get; set; }

    public int? PatronId { get; set; }

    public string? Details { get; set; }

    public int? MedicalConditionMaintenanceId { get; set; }

    public virtual MedicalConditionMaintenance? MedicalConditionMaintenance { get; set; }

    public virtual Patron? Patron { get; set; }
}
