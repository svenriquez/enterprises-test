using enterprises_test.Models.ViewModels;
using enterprises_test.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace enterprises_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private IEmployeeService EmployeeService { get; set; }
        private readonly IConfiguration Configuration;

        public EmployeeController(IConfiguration _Configuration,
                                IEmployeeService _EmployeeService)
        {
            this.EmployeeService = _EmployeeService;
            this.Configuration = _Configuration;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<ActionResult<PagedDataVMR<EmployeeVMR>>> GetAll(int? size, int? pageNumber, string textFilter)
        {
            try
            {
                PagedDataVMR<EmployeeVMR> result = new PagedDataVMR<EmployeeVMR>();
                result = await EmployeeService.GetAll(size, pageNumber, textFilter);

                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("get-by-id/{id}")]
        public async Task<ActionResult<EmployeeVMR>> GetById(long id)
        {
            try
            {
                EmployeeVMR item = new EmployeeVMR();
                item = await EmployeeService.GetById(id);

                return item;
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("get-form-data")]
        public async Task<ActionResult<EmployeeFormDataVMR>> GetFormData(long? id)
        {
            try
            {
                EmployeeFormDataVMR item = new EmployeeFormDataVMR();
                item = await EmployeeService.GetFormData(id);

                return item;
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost]
        [Route("post")]
        public async Task<ActionResult> Post([FromBody] Employee item)
        {
            try
            {
                await EmployeeService.Post(item);
                return Ok(new { mensaje = "Registro insertado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> Update([FromBody] Employee item)
        {
            try
            {
                await EmployeeService.Update(item);
                return Ok(new { mensaje = "Registro actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await EmployeeService.Delete(id);
                return Ok(new { mensaje = "Registro eliminado exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
