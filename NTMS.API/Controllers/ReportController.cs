using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NTMS.API.Utility;
using NTMS.BLL.Services.Abstract;
using NTMS.DTO;

namespace NTMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        [HttpGet,Route("Report")]
        public async Task<IActionResult> Report(int tenantId, string firstDate, string lastDate)
        {
            var rsp = new Response<ReportDTO>();
            try
            {
                rsp.status = true;
                rsp.value = await _reportService.GetByTenantIdAndDateRange(tenantId, firstDate, lastDate);
                //rsp.value = await _reportService.Report(tenantId, firstDate, lastDate);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }
    }
}
