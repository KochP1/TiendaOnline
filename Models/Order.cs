using System;
using System.Collections.Generic;

namespace TiendaOnline.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public int CartId { get; set; }

    public string OrderNumber { get; set; } = null!;

    public DateTime? OrderDate { get; set; }

    public decimal OrderTotal { get; set; }

    public string? Status { get; set; }

    public string ShippingAddress { get; set; } = null!;

    public string BillingAddress { get; set; } = null!;

    public virtual Cart Cart { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Shipping> Shippings { get; set; } = new List<Shipping>();

    public virtual User User { get; set; } = null!;
}
