using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TiendaOnline.Models;

public partial class User
{
    public int UserId { get; set; }

    public required string Email { get; set; } = null!;

    public required string PasswordHash { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? LastLogin { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
