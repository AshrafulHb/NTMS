using System;
using System.Collections.Generic;

namespace NTMS.Model;

public partial class Flat
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public decimal? Rent { get; set; }
    public decimal? GasBill { get; set; }

    public decimal? CleanerBill { get; set; }


    public virtual ICollection<Emeter> Emeters { get; set; } = new List<Emeter>();

    public virtual ICollection<Tenant> Tenants { get; set; } = new List<Tenant>();

    public virtual ICollection<Wmeter> Wmeters { get; set; } = new List<Wmeter>();

}
