using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class MedicalConditionMaintenance
{
    public int MedicalConditionMaintenanceId { get; set; }

    public string? Category { get; set; }

    public virtual ICollection<MedicalCondition> MedicalConditions { get; set; } = new List<MedicalCondition>();
}
