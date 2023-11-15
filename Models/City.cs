using System;
using System.Collections.Generic;

namespace Weather_Deatils.Models;

public partial class City
{
    public int CityId { get; set; }

    public string? CityName { get; set; }

    public string? Country { get; set; }

    public int? UserId { get; set; }

    public virtual UserCity? User { get; set; }
}
