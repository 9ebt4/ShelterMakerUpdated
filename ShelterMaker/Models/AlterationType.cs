using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class AlterationType
{
    public int AlterationTypeId { get; set; }

    public string? Category { get; set; }

    public virtual ICollection<Alteration> Alterations { get; set; } = new List<Alteration>();
}
