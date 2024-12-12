using AutoMapper;
using NTMS.BLL.Services.Abstract;
using NTMS.DAL.Repository.Abstract;
using NTMS.DTO;
using NTMS.Model;
using System.Globalization;

namespace NTMS.BLL.Services
{
    public class ReportService : IReportService
    {
        private readonly IGenericRepository<Tenant> _tenantRepository;
        private readonly IGenericRepository<Flat> _flatRepository;
        private readonly IGenericRepository<Emeter> _meterRepository;
        private readonly IGenericRepository<Ereading> _ereadingRepository;
        private readonly IGenericRepository<EbillingRule> _ruleRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;

        public ReportService(
            IGenericRepository<Tenant> tenantRepository,
            IGenericRepository<Flat> flatRepository,
            IGenericRepository<Ereading> ereadingRepository,
            IGenericRepository<Emeter> meterRepository,
            IGenericRepository<EbillingRule> ebillingRuleRepository,
            IReportRepository reportRepository,
            IMapper mapper)
        {
            _tenantRepository = tenantRepository;
            _flatRepository = flatRepository;
            _ereadingRepository = ereadingRepository;
            _meterRepository = meterRepository;
            _ruleRepository = ebillingRuleRepository;
            _reportRepository = reportRepository;
            _mapper = mapper;
        }



        public async Task<ReportDTO> GetByTenantIdAndDateRange(int tenantId, string firstDate, string lastDate)
        {
            try
            {
                var startDate = DateTime.ParseExact(firstDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var endDate = DateTime.ParseExact(lastDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var report = await _reportRepository.GetByTenantIdAndDateRangeAsync(tenantId, startDate, endDate);
                return _mapper.Map<ReportDTO>(report);

            }
            catch (Exception ex) { throw; }
        }
        /*    private async Task<decimal> CalculateElectricityCharge(int consumedUnits, EbillingRule ebr, bool isShop)
            {
                if (isShop)
                {
                    return consumedUnits * ebr.MinimumCharge;
                }

                var slabs = new[] { ebr.Rate1, ebr.Rate2, ebr.Rate3, ebr.Rate4 };
                var thresholds = new[] { ebr.To1, ebr.To2, ebr.To3 };
                var unitsInSlabs = new int[slabs.Length];
                var remainingUnits = consumedUnits;

                for (int i = 0; i < thresholds.Length && remainingUnits > 0; i++)
                {
                    var slabUnits = i == 0 ? thresholds[i] : thresholds[i] - thresholds[i - 1];
                    unitsInSlabs[i] = Math.Min(slabUnits, remainingUnits);
                    remainingUnits -= unitsInSlabs[i];
                }

                //unitsInSlabs[3]=remainingUnits;
                unitsInSlabs[^1] = remainingUnits; // Remaining units beyond all thresholds.
                return unitsInSlabs.Select((units, index) => units * slabs[index]).Sum();
            }
            public async Task<ReportDTO> Report(int tenantId, string firstDate, string lastDate)
            {
                // Parse dates from input strings
                var fDate = DateTime.ParseExact(firstDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var lDate = DateTime.ParseExact(lastDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                // Retrieve tenant and associated data
                var tenant = await _tenantRepository.Get(t => t.Id == tenantId && t.IsActive)
                    ?? throw new InvalidOperationException("No active tenant found.");

                var flat = await _flatRepository.Get(f => f.Id == tenant.FlatId)
                    ?? throw new InvalidOperationException("No flat associated with the tenant found.");

                var eMeter = await _meterRepository.Get(m => m.FlatId == flat.Id && m.IsActive == true);
                var ebr = (await _ruleRepository.GetAll()).FirstOrDefault()
                    ?? throw new InvalidOperationException("No billing rules configured.");

                // Initialize variables
                decimal electricityCharge = 0, demandCharge = 0, serviceCharge = 0;
                decimal principalAmount = 0, vat = 0, electricityBill = 0;
                int cr = 0, pr = 0, consumedUnits = 0;
                DateTime sd = fDate, ed = lDate;

                if (eMeter != null)
                {
                    var readings = await _ereadingRepository.GetAll(r =>
                        r.EmeterId == eMeter.Id &&
                        ((r.StartDate >= fDate && r.StartDate <= lDate) ||
                         (r.EndDate >= fDate && r.EndDate <= lDate)));

                    var reading = readings.Include(r => r.Emeter).FirstOrDefault()
                        ?? throw new InvalidOperationException("No readings found within the specified period.");

                    // Set billing period details
                    sd = reading.StartDate;
                    ed = reading.EndDate;
                    cr = reading.CurrentReading;
                    pr = reading.PreviousReading;
                    consumedUnits = cr - pr;

                    // Calculate electricity charges
                    bool isShop = flat.Code.StartsWith("Shop");
                    electricityCharge = await CalculateElectricityCharge(consumedUnits, ebr, isShop);
                    demandCharge = ebr.DemandCharge;
                    serviceCharge = ebr.ServiceCharge;

                    principalAmount = electricityCharge + demandCharge + serviceCharge;
                    vat = Math.Round(principalAmount * ebr.Vat / 100, 2);
                    electricityBill = principalAmount + vat;
                }

                // Retrieve utility costs
                var utilityOptions = (await _utilityRepository.GetAll()).ToDictionary(o => o.Name);
                if (!utilityOptions.TryGetValue("Gas", out var gasOption) || !utilityOptions.TryGetValue("Cleaner", out var cleanerOption))
                {
                    throw new InvalidOperationException("Required utility options (Gas and Cleaner) not configured.");
                }

                decimal gasBill = gasOption.Cost;
                decimal cleanerBill = cleanerOption.Cost;
                decimal houseRent = flat.Rent.GetValueOrDefault(0);
                decimal totalBill = electricityBill + gasBill + cleanerBill + houseRent;

                // Return the report DTO
                return new ReportDTO
                {
                    TenantName = tenant.Name,
                    BillStartDate = sd.ToString("dd/MM/yyyy"),
                    BillEndDate = ed.ToString("dd/MM/yyyy"),
                    BillingPeriod = sd.ToString("MMMM"),
                    ElectricMeterNo = eMeter?.MeterNumber ?? "N/A",
                    ElectricMeterCurrentReading = cr.ToString(),
                    ElectricMeterPreviousReading = pr.ToString(),
                    ConsumedElectricUnit = consumedUnits.ToString(),
                    ElectricityCharge = electricityCharge.ToString("0.00"),
                    DemandCharge = demandCharge.ToString("0.00"),
                    MeterRent = serviceCharge.ToString("0.00"),
                    PrincipalAmount = principalAmount.ToString("0.00"),
                    Vat = vat.ToString("0.00"),
                    ElectricityBill = electricityBill.ToString("0.00"),
                    GasBill = gasBill.ToString("0.00"),
                    CleanerBill = cleanerBill.ToString("0.00"),
                    HouseRent = houseRent.ToString("0.00"),
                    Total = totalBill.ToString("0.00"),
                    FlatCode = flat.Code
                };
            }*/


    }
}

