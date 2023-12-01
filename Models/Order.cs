using System;
using System.Collections.Generic;

namespace Weather_Deatils.Models;

public partial class Order
{
    public int CityId { get; set; }

    public int? UserId { get; set; }

    public string? Cityname { get; set; }

    public virtual UserCity? User { get; set; }
}
