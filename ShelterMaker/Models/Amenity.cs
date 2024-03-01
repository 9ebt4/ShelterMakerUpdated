using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class Amenity
{
    public int AmenityId { get; set; }

    public int? BedId { get; set; }

    public string? Category { get; set; }

    public virtual Bed? Bed { get; set; }
}
