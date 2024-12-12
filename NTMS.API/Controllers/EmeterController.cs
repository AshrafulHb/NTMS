using Microsoft.AspNetCore.Mvc;
using NTMS.API.Utility;
using NTMS.BLL.Services.Abstract;
using NTMS.DTO;

namespace NTMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmeterController : ControllerBase
    {
        private readonly IEmeterService _emeterService;
        public EmeterController(IEmeterService emeterService)
        {
            _emeterService = emeterService;
        }
        [HttpGet, Route("List")]
        public async Task<IActionResult> List()
        {
            var rsp = new Response<List<EmeterDTO>>();

            try
            {
                rsp.status = true;
                rsp.value = await _emeterService.List();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }
        [HttpGet, Route("ListActive")]
        public async Task<IActionResult> ListActive()
        {
            var rsp = new Response<List<EmeterDTO>>();

            try
            {
                rsp.status = true;
                rsp.value = await _emeterService.ListActive();
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
            var rsp = new Response<EmeterDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _emeterService.Get(id);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }
        [HttpPost, Route("Create")]
        public async Task<IActionResult> Create([FromBody] EmeterDTO meter)
        {
            var rsp = new Response<EmeterDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _emeterService.Create(meter);
            }
            catch (Exception ex) { rsp.status = false; rsp.msg = ex.Message; }
            return Ok(rsp);
        }

        [HttpPut, Route("Edit")]
        public async Task<IActionResult> Edit([FromBody] EmeterDTO meter)
        {
            var rsp = new Response<bool>();

            try
            {
                rsp.status = true;
                rsp.value = await _emeterService.Edit(meter);
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
                rsp.value = await _emeterService.Delete(id);
            }
            catch (Exception ex) { rsp.status = false; rsp.msg = ex.Message; }
            return Ok(rsp);
        }
    }
}
