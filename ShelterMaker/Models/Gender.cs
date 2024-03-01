using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class Gender
{
    public int GenderId { get; set; }

    public string? Category { get; set; }

    public virtual ICollection<Person> People { get; set; } = new List<Person>();
}
