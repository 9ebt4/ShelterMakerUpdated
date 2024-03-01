using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class Alteration
{
    public int AlterationId { get; set; }

    public int? AssociateId { get; set; }

    public int? AlterationTypeId { get; set; }

    public int? TrackedTableId { get; set; }

    public DateTime? AlterationDate { get; set; }

    public int? AlteredKey { get; set; }

    public virtual AlterationType? AlterationType { get; set; }

    public virtual Associate? Associate { get; set; }

    public virtual TrackedTable? TrackedTable { get; set; }
}
