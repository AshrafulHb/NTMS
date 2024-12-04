using NTMS.DTO;

namespace NTMS.BLL.Services.Abstract
{
    public interface IReportService
    {
        Task<ReportDTO> Report(int tenantId, string firstDate, string lastDate);
        Task<ReportDTO> GetByTenantIdAndDateRange(int tenantId, string firstDate, string lastDate);


    }
}
