﻿using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class Checklist
{
    public int ChecklistId { get; set; }

    public int FacilityId { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public string Options { get; set; } = null!;

    public virtual Facility Facility { get; set; } = null!;

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
