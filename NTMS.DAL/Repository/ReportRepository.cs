using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NTMS.DAL.DBContext;
using NTMS.DAL.Repository.Abstract;
using NTMS.Model;

namespace NTMS.DAL.Repository
{
    public class ReportRepository : GenericRepository<Report> , IReportRepository
    {

        public ReportRepository(NtmsContext context)
            : base(context) { }

        public async Task<Report> GetByTenantIdAndDateRangeAsync(int tenantId, DateTime startDate, DateTime endDate)
        {
            try
            {

                
                var sql = @" EXEC [dbo].[GetReportByTenantIdAndDateRange] @TenantId, @StartDate, @EndDate";

                var parameters = new[]
                {
                    new SqlParameter("@TenantId", tenantId),
                    new SqlParameter("@StartDate", startDate),
                    new SqlParameter("@EndDate", endDate)
                };


                var result  = (await Context.Database.SqlQueryRaw<Report>(sql, parameters).ToListAsync()).FirstOrDefault();
                return result;
            }
            catch(Exception ex)
            {
                throw;
            }


        }
    }
}
