using NTMS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTMS.BLL.Services.Abstract
{
    public interface IReportService
    {
        Task<ReportDTO> Report(int tenantId, string firstDate, string lastDate);
        Task<ReportDTO> GetByTenantIdAndDateRange(int tenantId, string firstDate, string lastDate);

    }
}
