using System;
using System.Threading.Tasks;
using enterprises_test.Models.ViewModels;
using enterprises_test.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace enterprises_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnterpriseController : ControllerBase
    {
        private IEnterpriseService EnterpriseService { get; set; }
        private readonly IConfiguration Configuration;

        public EnterpriseController(IConfiguration _Configuration,
                                IEnterpriseService _EnterpriseService)
        {
            this.EnterpriseService = _EnterpriseService;
            this.Configuration = _Configuration;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<ActionResult<PagedDataVMR<Enterprise>>> GetAll(int? size, int? pageNumber, string textFilter)
        {
            try
            {
                PagedDataVMR<Enterprise> result = new PagedDataVMR<Enterprise>();
                result = await EnterpriseService.GetAll(size, pageNumber, textFilter);

                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("get-by-id/{id}")]
        public async Task<ActionResult<Enterprise>> GetById(long id)
        {
            try
            {
                Enterprise item = new Enterprise();
                item = await EnterpriseService.GetById(id);

                return item;
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost]
        [Route("post")]
        public async Task<ActionResult> Post([FromBody] Enterprise item)
        {
            try
            {
                await EnterpriseService.Post(item);
                return Ok(new { mensaje = "Registro insertado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> Update([FromBody] Enterprise item)
        {
            try
            {
                await EnterpriseService.Update(item);
                return Ok(new { mensaje = "Registro actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult> Delete(long id )
        {
            try
            {
                await EnterpriseService.Delete(id);
                return Ok(new { mensaje = "Registro eliminado exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}