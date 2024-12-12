using System;
using System.Collections.Generic;

namespace NTMS.Model;

public partial class Tenant
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Occupation { get; set; }

    public string? Paddress { get; set; }

    public string? Telephone { get; set; }
    public string? SecurityDeposit { get; set; }

    public DateTime StartDate { get; set; }

    public bool IsActive { get; set; }

    public int? FlatId { get; set; }

    public virtual Flat? Flat { get; set; }
}
