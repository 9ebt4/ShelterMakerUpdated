using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class Bed
{
    public int BedId { get; set; }

    public int? FacilityId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Amenity> Amenities { get; set; } = new List<Amenity>();

    public virtual ICollection<BedMaintenance> BedMaintenances { get; set; } = new List<BedMaintenance>();

    public virtual Facility? Facility { get; set; }

    public virtual ICollection<Patron> Patrons { get; set; } = new List<Patron>();
}
