using System;
using System.Collections.Generic;

namespace DemoApp.Entities;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductArticle { get; set; }

    public string? ProductName { get; set; }

    public decimal? MinimumCostForPartner { get; set; }

    public int? ProductTypeId { get; set; }

    public int? MaterialTypeId { get; set; }

    public virtual MaterialType? MaterialType { get; set; }

    public virtual ProductType? ProductType { get; set; }

    public virtual ICollection<ProductWorkshop> ProductWorkshops { get; set; } = new List<ProductWorkshop>();
}
