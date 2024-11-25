using NTMS.Model;

namespace NTMS.DAL.Repository.Abstract
{
    public interface IReportRepository : IGenericRepository<Report>
    {
        Task<Report> GetByTenantIdAndDateRangeAsync(int tenantId, DateTime startDate, DateTime endDate);
    }
}
