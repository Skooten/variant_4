using System;
using System.Collections.Generic;

namespace DemoApp.Entities;

public partial class Workshop
{
    public int WorkshopId { get; set; }

    public string? WorkshopName { get; set; }

    public string? WorkshopType { get; set; }

    public int? WorkersCount { get; set; }

    public virtual ICollection<ProductWorkshop> ProductWorkshops { get; set; } = new List<ProductWorkshop>();
}
