using System;
using System.Collections.Generic;

namespace ShelterMaker.Models;

public partial class Item
{
    public int ItemId { get; set; }

    public bool IsChecked { get; set; }

    public string? Content { get; set; }

    public int? CheckListId { get; set; }

    public virtual Checklist? CheckList { get; set; }
}
