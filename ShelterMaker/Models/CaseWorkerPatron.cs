using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class CaseWorkerPatron
{
    public int CaseWorkerPatronId { get; set; }

    public int? PatronId { get; set; }

    public int? CaseWorkerId { get; set; }

    public virtual Person? CaseWorker { get; set; }

    public virtual Patron? Patron { get; set; }
}
