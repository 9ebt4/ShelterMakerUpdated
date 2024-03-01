using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class PatronBan
{
    public int PatronBanId { get; set; }

    public int? PatronId { get; set; }

    public int? BanId { get; set; }

    public virtual Ban? Ban { get; set; }

    public virtual Patron? Patron { get; set; }
}
