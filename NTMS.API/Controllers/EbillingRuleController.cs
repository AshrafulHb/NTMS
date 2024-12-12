using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NTMS.API.Utility;
using NTMS.BLL.Services.Abstract;
using NTMS.DTO;

namespace NTMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EbillingRuleController : ControllerBase
    {
        private readonly IEruleService _eruleService;

        public EbillingRuleController(IEruleService eruleService)
        {
            _eruleService = eruleService;
        }
        [HttpGet("GetRule")]
        public async Task<IActionResult> GetRule()
        {
            var rsp = new Response<EbillingRuleDTO>();
            try
            {
                rsp.status = true;
                rsp.value = await _eruleService.Get();

            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpPost, Route("Create")]
        public async Task<IActionResult> Create([FromBody] EbillingRuleDTO model)
        {
            var rsp = new Response<EbillingRuleDTO>();
            try
            {
                rsp.status = true;
                rsp.value = await _eruleService.Create(model);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }


        [HttpPut, Route("Edit")]
        public async Task<IActionResult> Edit([FromBody] EbillingRuleDTO model)
        {
            var rsp = new Response<bool>();

            try
            {
                rsp.status = true;
                rsp.value = await _eruleService.Edit(model);
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
                rsp.value = await _eruleService.Delete(id);
            }
            catch (Exception ex) { rsp.status = false; rsp.msg = ex.Message; }
            return Ok(rsp);
        }
    }
}
