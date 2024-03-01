using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class EmergencyContact
{
    public int EmergencyContactId { get; set; }

    public int? AssociateId { get; set; }

    public int? PatronId { get; set; }

    public int? PersonId { get; set; }

    public int? RelationshipId { get; set; }

    public virtual Associate? Associate { get; set; }

    public virtual Patron? Patron { get; set; }

    public virtual Person? Person { get; set; }

    public virtual Relationship? Relationship { get; set; }
}
