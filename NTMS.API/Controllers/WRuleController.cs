using Microsoft.AspNetCore.Mvc;
using NTMS.API.Utility;
using NTMS.BLL.Services.Abstract;
using NTMS.DTO;

namespace NTMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WRuleController : ControllerBase
    {
        private readonly IWruleService _wruleService;

        public WRuleController(IWruleService wruleService)
        {
            _wruleService = wruleService;
        }
        [HttpGet("GetRule")]
        public async Task<IActionResult> GetRule()
        {
            var rsp = new Response<WbillingRuleDTO>();
            try
            {
                rsp.status = true;
                rsp.value = await _wruleService.Get();

            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpPost, Route("Create")]
        public async Task<IActionResult> Create([FromBody] WbillingRuleDTO model)
        {
            var rsp = new Response<WbillingRuleDTO>();
            try
            {
                rsp.status = true;
                rsp.value = await _wruleService.Create(model);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }


        [HttpPut, Route("Edit")]
        public async Task<IActionResult> Edit([FromBody] WbillingRuleDTO model)
        {
            var rsp = new Response<bool>();

            try
            {
                rsp.status = true;
                rsp.value = await _wruleService.Edit(model);
            }
            catch (Exception ex) { rsp.status = false; rsp.msg = ex.Message; }
            return Ok(rsp);
        }

        [HttpDelete, Route("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var rsp = new Response<bool>();

            try
            {
                rsp.status = true;
                rsp.value = await _wruleService.Delete(id);
            }
            catch (Exception ex) { rsp.status = false; rsp.msg = ex.Message; }
            return Ok(rsp);
        }
    }
}
