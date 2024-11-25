using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        private readonly IGenericRepository<UtilityOption> _utilityRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;

        public ReportService(
            IGenericRepository<Tenant> tenantRepository,
            IGenericRepository<Flat> flatRepository,
            IGenericRepository<Ereading> ereadingRepository,
            IGenericRepository<Emeter> meterRepository,
            IGenericRepository<EbillingRule> ebillingRuleRepository,
            IGenericRepository<UtilityOption> utilityRepository,
            IReportRepository reportRepository,
            IMapper mapper)
        {
            _tenantRepository = tenantRepository;
            _flatRepository = flatRepository;
            _ereadingRepository = ereadingRepository;
            _meterRepository = meterRepository;
            _ruleRepository = ebillingRuleRepository;
            _utilityRepository = utilityRepository;
            _reportRepository = reportRepository ?? throw new ArgumentNullException(nameof(reportRepository));
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
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<decimal> CalculateElectricityCharge(int consumedUnits, EbillingRule ebr, bool isShop)
        {
            if (isShop)
            {
                return consumedUnits * ebr.MinimumCharge;
            }

            var slabs = new[] { ebr.Rate1, ebr.Rate2, ebr.Rate3, ebr.Rate4 };
            var thresholds = new[] { ebr.To1, ebr.To2, ebr.To3 };
            var unitsInSlabs = new int[4];
            var remainingUnits = consumedUnits;

            for (int i = 0; i < thresholds.Length && remainingUnits > 0; i++)
            {
                var slabUnits = i == 0 ? thresholds[i] : thresholds[i] - thresholds[i - 1];
                unitsInSlabs[i] = Math.Min(slabUnits, remainingUnits);
                remainingUnits -= unitsInSlabs[i];
            }

            unitsInSlabs[3] = remainingUnits; // Remaining units beyond all thresholds.
            return unitsInSlabs.Select((units, index) => units * slabs[index]).Sum();
        }

        public async Task<ReportDTO> Report(int tenantId, string firstDate, string lastDate)
        {
            var fDate = DateTime.ParseExact(firstDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var lDate = DateTime.ParseExact(lastDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var ebr = (await _ruleRepository.GetAll()).FirstOrDefault()
                      ?? throw new InvalidOperationException("No bill rule found");

            var utilityOptions = (await _utilityRepository.GetAll()).ToDictionary(o => o.Name);
            if (!utilityOptions.ContainsKey("Gas") || !utilityOptions.ContainsKey("Cleaner"))
            {
                throw new InvalidOperationException("Required utility options not found");
            }

            var tenant = await _tenantRepository.Get(t => t.Id == tenantId && t.IsActive)
                         ?? throw new InvalidOperationException("No active tenant found");

            var flat = await _flatRepository.Get(f => f.Id == tenant.FlatId)
                        ?? throw new InvalidOperationException("No flat found");

            var eMeter = await _meterRepository.Get(m => m.FlatId == flat.Id)
                          ?? throw new InvalidOperationException("No electric meter found");

            var readings = await _ereadingRepository.GetAll(r =>
                r.EmeterId == eMeter.Id &&
                ((r.StartDate >= fDate && r.StartDate <= lDate) ||
                 (r.EndDate >= fDate && r.EndDate <= lDate)));

            var reading = readings.Include(r => r.Emeter).FirstOrDefault()
                          ?? throw new InvalidOperationException("No reading found");

            int consumedUnits = reading.CurrentReading - reading.PreviousReading;
            bool isShop = flat.Code.StartsWith("Shop");

            decimal electricityCharge = await CalculateElectricityCharge(consumedUnits, ebr, isShop);
            decimal demandCharge = ebr.DemandCharge;
            decimal serviceCharge = ebr.ServiceCharge;

            decimal principalAmount = electricityCharge + demandCharge + serviceCharge;
            decimal vat = Math.Round(principalAmount * ebr.Vat / 100);
            decimal electricityBill = principalAmount + vat;
            decimal houseRent = flat.Rent.Value;

            decimal gasBill = utilityOptions["Gas"].Cost;
            decimal cleanerBill = utilityOptions["Cleaner"].Cost;
            decimal totalBill = (decimal)(electricityBill + gasBill + cleanerBill + flat.Rent);

            return new ReportDTO
            {
                TenantName = tenant.Name,
                BillStartDate = reading.StartDate.ToString("dd/MM/yyyy"),
                BillEndDate = reading.EndDate.ToString("dd/MM/yyyy"),
                BillingPeriod = reading.StartDate.ToString("MMMM"),
                ElectricMeterNo = eMeter.MeterNumber,
                ElectricMeterCurrentReading = reading.CurrentReading.ToString(),
                ElectricMeterPreviousReading = reading.PreviousReading.ToString(),
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
        }
    }
}




/*using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        private readonly IGenericRepository<UtilityOption> _utilityRepository;
        private readonly IMapper _mapper;

        public ReportService(IGenericRepository<Tenant> tenantRepository, IGenericRepository<Flat> flatRepository, IGenericRepository<Ereading> ereadingRepository,
            IGenericRepository<Emeter> meterRepository, IGenericRepository<EbillingRule> ebillingRuleRepository, IGenericRepository<UtilityOption> utilityRepository, IMapper mapper)
        {
            _tenantRepository = tenantRepository;
            _flatRepository = flatRepository;
            _ereadingRepository = ereadingRepository;

            _meterRepository = meterRepository;
            _ruleRepository = ebillingRuleRepository;
            _utilityRepository = utilityRepository;
            _mapper = mapper;
        }
        private async Task<decimal> ElectricityCharge(int consumedUnit, bool x = false)
        {
            var slab1 = 0.00M;
            var slab2 = 0.00M;
            var slab3 = 0.00M;
            var slab4 = 0.00M;
            var eCharge = 0.00M;

            var query = await _ruleRepository.GetAll();
            var ebr = query.FirstOrDefault();




            if (ebr != null && (!x))
            {
                slab1 = ebr.Rate1;
                slab2 = ebr.Rate2;
                slab3 = ebr.Rate3;
                slab4 = ebr.Rate4;
                int x1 = 0, x2 = 0, x3 = 0, x4 = 0;
                if (consumedUnit > ebr.To3 *//*300*//*)
                {
                    x1 = ebr.To1 - ebr.From1;//100;
                    x2 = ebr.To2 - ebr.To1;//100;
                    x3 = ebr.To3 - ebr.To2; //100;
                    x4 = consumedUnit - ebr.To3;//300;
                }
                else if (consumedUnit > ebr.To2 *//*200*//* && consumedUnit <= ebr.To3 *//* 300*//*)
                {
                    x1 = ebr.To1 - ebr.From1;//100;
                    x2 = ebr.To2 - ebr.To1;//100;
                    x3 = consumedUnit - ebr.To2; //100;
                    x4 = 0;//300;

                }
                else if (consumedUnit > ebr.From2 *//*100*//* && consumedUnit <= ebr.From3 *//* 200*//*)
                {
                    x1 = ebr.To1 - ebr.From1;//100;
                    x2 = consumedUnit - ebr.To1;//100;
                    x3 = 0;
                    x4 = 0;
                }
                else
                {
                    x1 = consumedUnit;
                    x2 = 0;
                    x3 = 0;
                    x4 = 0;
                }
                eCharge = x1 * slab1 + x2 * slab2 + x3 * slab3 + x4 * slab4;
                return eCharge;

            }
            else if (ebr != null & x)
            {
                eCharge = consumedUnit * ebr.MinimumCharge;
            }
            return eCharge;
        }
        public async Task<ReportDTO> Report(int tenantId, string firstDate, string lastDate)
        {
            DateTime fDate = DateTime.ParseExact(firstDate, "dd/MM/yyyy", new CultureInfo("en-US"));
            DateTime lDate = DateTime.ParseExact(lastDate, "dd/MM/yyyy", new CultureInfo("en-US"));

            ReportDTO reportDTO = new ReportDTO();

            try
            {
                var ebrQuery = await _ruleRepository.GetAll();
                if (ebrQuery == null) throw new TaskCanceledException("No bill rule found");
                var ebr = ebrQuery.FirstOrDefault();

                var billOption = await _utilityRepository.GetAll(); if (billOption == null) throw new TaskCanceledException("No utility found");
                var option = billOption.Where(g => g.Name == "Gas").First();
                var cleaner = billOption.First(c => c.Name == "Cleaner");

                var tenant = await _tenantRepository.Get(t => t.Id == tenantId && t.IsActive == true);
                if (tenant == null) throw new TaskCanceledException("No tenant found");

                var flat = await _flatRepository.Get(f => f.Id == tenant.FlatId);
                if (flat == null) throw new TaskCanceledException("No flat found");
                var eMeter = await _meterRepository.Get(m => m.FlatId == flat.Id);
                if (eMeter == null) throw new TaskCanceledException("No electric meter found");

                bool x = (flat.Code == "Shop1" || flat.Code == "Shop 2") ? true : false;

                var query = await _ereadingRepository.GetAll(r => r.EmeterId == eMeter.Id && ((r.StartDate >= fDate && r.StartDate <= lDate) || (r.EndDate >= fDate && r.EndDate <= lDate)));
                var reading = query.Include(r => r.Emeter).FirstOrDefault();
                if (reading == null) throw new TaskCanceledException("No reading found");
                int cUnit = reading.CurrentReading - reading.PreviousReading;
                decimal ec = await ElectricityCharge(cUnit, x);
                var dc = ebr.DemandCharge;
                var sc = ebr.ServiceCharge;
                var pa = ec + dc + sc;
                var vat = Math.Round(pa / 100 * ebr.Vat);
                var eb = pa + vat;
                var gb = option.Cost;
                var bp = reading.StartDate.ToString("MMMM");
                var total = (eb + gb + cleaner.Cost + flat.Rent);
                reportDTO.TenantName = tenant.Name;
                reportDTO.FlatCode = flat.Code;
                reportDTO.BillingPeriod = bp;
                reportDTO.BillStartDate = reading.StartDate.ToString("dd/MM/yyyy");
                reportDTO.BillEndDate = reading.EndDate.ToString("dd/MM/yyyy");
                reportDTO.ElectricMeterNo = eMeter.MeterNumber;
                reportDTO.ElectricMeterCurrentReading = reading.CurrentReading.ToString();
                reportDTO.ElectricMeterPreviousReading = reading.PreviousReading.ToString();
                reportDTO.ConsumedElectricUnit = cUnit.ToString();
                reportDTO.ElectricityCharge = ec.ToString();
                reportDTO.DemandCharge = dc.ToString("##.00");
                reportDTO.MeterRent = sc.ToString("##.00");
                reportDTO.PrincipalAmount = pa.ToString("##.00");
                reportDTO.Vat = vat.ToString("##.00");
                reportDTO.ElectricityBill = eb.ToString("##.00");
                reportDTO.GasBill = option.Cost.ToString("##.00");
                reportDTO.CleanerBill = cleaner.Cost.ToString("##.00");
                reportDTO.HouseRent = flat.Rent.ToString();
                reportDTO.Total = total.ToString();

                return reportDTO;
            }
            catch { throw; }
        }
    }
}*/
