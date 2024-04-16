using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class GoogleUser
{
    public int GoogleUserId { get; set; }

    public string GoogleToken { get; set; } = null!;

    public bool IsActive { get; set; }

    public int? PersonId { get; set; }

    public int? FacilityId { get; set; }

    public virtual ICollection<Associate> Associates { get; set; } = new List<Associate>();

    public virtual Facility? Facility { get; set; }

    public virtual Person? Person { get; set; }
}
