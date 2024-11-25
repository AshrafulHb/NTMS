namespace NTMS.Model;

public partial class Report
{

    public string? TenantName { get; set; }

    public string? FlatCode { get; set; }

    public string? BillingPeriod { get; set; }

    public string? BillStartDate { get; set; }

    public string? BillEndDate { get; set; }

    public string? ElectricMeterNo { get; set; }

    public string? ElectricMeterCurrentReading { get; set; }

    public string? ElectricMeterPreviousReading { get; set; }

    public string? ConsumedElectricUnit { get; set; }

    public string? ElectricityCharge { get; set; }

    public string? DemandCharge { get; set; }

    public string? MeterRent { get; set; }

    //       public string? MinimumCharge { get; set; }

    public string? PrincipalAmount { get; set; }

    public string? Vat { get; set; }

    public string? ElectricityBill { get; set; }


    public string? HouseRent { get; set; }

    public string? GasBill { get; set; }

    public string? CleanerBill { get; set; }

    public string? Total { get; set; }
}
