using enterprises_test.Models.ViewModels;
using enterprises_test.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace enterprises_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentEmployeeController : ControllerBase
    {
        private IDepartmentEmployeeService DepartmentEmployeeService { get; set; }
        private readonly IConfiguration Configuration;

        public DepartmentEmployeeController(IConfiguration _Configuration,
                                IDepartmentEmployeeService _DepartmentEmployeeService)
        {
            this.DepartmentEmployeeService = _DepartmentEmployeeService;
            this.Configuration = _Configuration;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<ActionResult<PagedDataVMR<DepartmentsEmployee>>> GetAll(int? size, int? pageNumber)
        {
            try
            {
                PagedDataVMR<DepartmentsEmployee> result = new PagedDataVMR<DepartmentsEmployee>();
                result = await DepartmentEmployeeService.GetAll(size, pageNumber);

                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("get-by-id/{id}")]
        public async Task<ActionResult<DepartmentsEmployee>> GetById(long id)
        {
            try
            {
                DepartmentsEmployee item = new DepartmentsEmployee();
                item = await DepartmentEmployeeService.GetById(id);

                return item;
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost]
        [Route("post")]
        public async Task<ActionResult> Post([FromBody] DepartmentsEmployee item)
        {
            try
            {
                await DepartmentEmployeeService.Post(item);
                return Ok(new { mensaje = "Registro insertado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> Update([FromBody] DepartmentsEmployee item)
        {
            try
            {
                await DepartmentEmployeeService.Update(item);
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
                await DepartmentEmployeeService.Delete(id);
                return Ok(new { mensaje = "Registro eliminado exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
