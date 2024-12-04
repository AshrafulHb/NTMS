using Microsoft.AspNetCore.Mvc;
using NTMS.API.Utility;
using NTMS.BLL.Services.Abstract;
using NTMS.DTO;

namespace NTMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WmeterController : ControllerBase
    {
        private readonly IWmeterService _wmeterService;

        public WmeterController(IWmeterService wmeterService)
        {
            _wmeterService = wmeterService;
        }
        [HttpGet, Route("List")]
        public async Task<IActionResult> List()
        {
            var rsp = new Response<List<WmeterDTO>>();

            try
            {
                rsp.status = true;
                rsp.value = await _wmeterService.List();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpGet, Route("Get/{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var rsp = new Response<WmeterDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _wmeterService.Get(id);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }
        [HttpPost, Route("Create")]
        public async Task<IActionResult> Create([FromBody] WmeterDTO meter)
        {
            var rsp = new Response<WmeterDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _wmeterService.Create(meter);
            }
            catch (Exception ex) { rsp.status = false; rsp.msg = ex.Message; }
            return Ok(rsp);
        }

        [HttpPut, Route("Edit")]
        public async Task<IActionResult> Edit([FromBody] WmeterDTO meter)
        {
            var rsp = new Response<bool>();

            try
            {
                rsp.status = true;
                rsp.value = await _wmeterService.Edit(meter);
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
                rsp.value = await _wmeterService.Delete(id);
            }
            catch (Exception ex) { rsp.status = false; rsp.msg = ex.Message; }
            return Ok(rsp);
        }
    }
}
