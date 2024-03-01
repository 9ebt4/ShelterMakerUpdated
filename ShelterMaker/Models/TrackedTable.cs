using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class TrackedTable
{
    public int TrackedTableId { get; set; }

    public string? TableName { get; set; }

    public virtual ICollection<Alteration> Alterations { get; set; } = new List<Alteration>();
}
