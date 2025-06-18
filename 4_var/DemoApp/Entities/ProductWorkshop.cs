using System;
using System.Collections.Generic;

namespace DemoApp.Entities;

public partial class ProductWorkshop
{
    public int ProductWorkshopId { get; set; }

    public int? ProductId { get; set; }

    public int? WorkshopId { get; set; }

    public decimal? ManufacturingTimeHours { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Workshop? Workshop { get; set; }
}
