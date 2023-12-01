using System;
using System.Collections.Generic;

namespace Weather_Deatils.Models;

public partial class UserCity
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
