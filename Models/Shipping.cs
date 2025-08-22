using System;
using System.Collections.Generic;

namespace TiendaOnline.Models;

public partial class Shipping
{
    public int ShippingId { get; set; }

    public int OrderId { get; set; }

    public string? TrackingNumber { get; set; }

    public string? Carrier { get; set; }

    public DateTime? ShippedDate { get; set; }

    public DateTime? EstimatedDelivery { get; set; }

    public DateTime? ActualDelivery { get; set; }

    public string? ShippingStatus { get; set; }

    public virtual Order Order { get; set; } = null!;
}
