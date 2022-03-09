using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using enterprises_test.Models.ViewModels;
using enterprises_test.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace enterprises_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private IDepartmentService DepartmentService { get; set; }
        private readonly IConfiguration Configuration;

        public DepartmentController(IConfiguration _Configuration,
                                IDepartmentService _DepartmentService)
        {
            this.DepartmentService = _DepartmentService;
            this.Configuration = _Configuration;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<ActionResult<PagedDataVMR<Department>>> GetAll(int? size, int? pageNumber, string textFilter)
        {
            try
            {
                PagedDataVMR<Department> result = new PagedDataVMR<Department>();
                result = await DepartmentService.GetAll(size, pageNumber, textFilter);

                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("get-by-id/{id}")]
        public async Task<ActionResult<Department>> GetById(long id)
        {
            try
            {
                Department item = new Department();
                item = await DepartmentService.GetById(id);

                return item;
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("get-form-data")]
        public async Task<ActionResult<DepartmentFormDataVMR>> GetFormData(long? id)
        {
            try
            {
                DepartmentFormDataVMR item = new DepartmentFormDataVMR();
                item = await DepartmentService.GetFormData(id);

                return item;
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("get-departments-by-idEnterprise/{id}")]
        public async Task<ActionResult<List<Department>>> GetDepartmentsByIdEnterprise(long id)
        {
            try
            {
                List<Department> resp = new List<Department>();
                resp = await DepartmentService.GetDepartmentsByIdEnterprise(id);

                return resp;
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost]
        [Route("post")]
        public async Task<ActionResult> Post([FromBody] Department item)
        {
            try
            {
                await DepartmentService.Post(item);
                return Ok(new { mensaje = "Registro insertado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> Update([FromBody] Department item)
        {
            try
            {
                await DepartmentService.Update(item);
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
                await DepartmentService.Delete(id);
                return Ok(new { mensaje = "Registro eliminado exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

    }
}