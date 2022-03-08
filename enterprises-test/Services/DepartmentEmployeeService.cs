using enterprises_test.Models;
using enterprises_test.Models.ViewModels;
using enterprises_test.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace enterprises_test.Services
{
    public class DepartmentEmployeeService : IDepartmentEmployeeService
    {
        readonly testContext context;

        public DepartmentEmployeeService(testContext _context)
        {
            context = _context;
        }

        public async Task<PagedDataVMR<DepartmentsEmployee>> GetAll(int? pageSize, int? page)
        {
            PagedDataVMR<DepartmentsEmployee> result = new PagedDataVMR<DepartmentsEmployee>();

            try
            {

                /*
                    public long IdDepartment { get; set; }
                    public long IdEmployee { get; set; }

                    public virtual Department IdDepartmentNavigation { get; set; }
                    public virtual Employee IdEmployeeNavigation { get; set; }
                 */
                var query = context.DepartmentsEmployees.Select(x => new DepartmentsEmployee()
                {
                    Id = x.Id,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedDate,
                    Status = x.Status,
                    IdDepartment = x.IdDepartment,
                    IdEmployee = x.IdEmployee,
                    IdDepartmentNavigation = new Department()
                    {
                        Id = x.IdDepartmentNavigation.Id,
                        Name = x.IdDepartmentNavigation.Name
                    },
                    IdEmployeeNavigation = new Employee()
                    {
                        Id = x.IdEmployeeNavigation.Id,
                        Name = x.IdEmployeeNavigation.Name
                    }
                });

                result.total = query.Count();

                result.elements = (page != null && pageSize != null) ?
                   await query.AsQueryable().
                   OrderBy(u => u.Id).Skip(page.Value * pageSize.Value).Take(pageSize.Value).ToListAsync() :
                       await query.ToListAsync();

                return result;
            }
            catch (SqlException ex)
            {
                string mensaje = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                throw new Exception("ErrorConexionBaseDatos: " + mensaje);
            }
            catch (Exception lex)
            {
                throw lex;
            }
        }

        public async Task<DepartmentsEmployee> GetById(long id)
        {
            try
            {
                DepartmentsEmployee DepartmentsEmployee = await context.DepartmentsEmployees.Where(x => x.Id == id).Select(x => new DepartmentsEmployee()
                {
                    Id = x.Id,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedDate,
                    Status = x.Status,
                    IdDepartment = x.IdDepartment,
                    IdEmployee = x.IdEmployee,
                    IdDepartmentNavigation = new Department()
                    {
                        Id = x.IdDepartmentNavigation.Id,
                        Name = x.IdDepartmentNavigation.Name
                    },
                    IdEmployeeNavigation = new Employee()
                    {
                        Id = x.IdEmployeeNavigation.Id,
                        Name = x.IdEmployeeNavigation.Name
                    }
                }).FirstOrDefaultAsync();

                return DepartmentsEmployee;
            }
            catch (SqlException ex)
            {
                string mensaje = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                throw new Exception("ErrorConexionBaseDatos: " + mensaje);
            }
            catch (Exception lex)
            {
                throw lex;
            }
        }

        public async Task Post(DepartmentsEmployee item)
        {
            try
            {
                item.CreatedDate = DateTime.Now;
                item.Status = true;

                context.DepartmentsEmployees.Add(item);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string mensaje = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                throw new Exception("ErrorConcurrencia: " + mensaje);
            }
            catch (DbUpdateException ex)
            {
                string mensaje = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                throw new Exception("ErrorIngresoDatos: " + mensaje);
            }
            catch (SqlException ex)
            {
                string mensaje = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                throw new Exception("ErrorConexionBaseDatos: " + mensaje);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task Update(DepartmentsEmployee item)
        {
            try
            {
                DepartmentsEmployee itemUpdate = context.DepartmentsEmployees.Find(item.Id);

                itemUpdate.ModifiedBy = item.ModifiedBy;
                itemUpdate.ModifiedDate = DateTime.Now;
                itemUpdate.Status = item.Status;

                itemUpdate.IdDepartment = item.IdDepartment;
                itemUpdate.IdEmployee = item.IdEmployee;

                context.Entry(itemUpdate).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string mensaje = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                throw new Exception("ErrorConcurrencia: " + mensaje);
            }
            catch (DbUpdateException ex)
            {
                string mensaje = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                throw new Exception("ErrorIngresoDatos: " + mensaje);
            }
            catch (SqlException ex)
            {
                string mensaje = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                throw new Exception("ErrorConexionBaseDatos: " + mensaje);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task Delete(long id)
        {
            try
            {
                DepartmentsEmployee itemDelete = context.DepartmentsEmployees.Find(id);

                if (itemDelete != null)
                {
                    context.Remove(itemDelete);
                    await context.SaveChangesAsync();
                }
            }
            catch (SqlException ex)
            {
                string mensaje = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                throw new Exception("ErrorConexionBaseDatos: " + mensaje);
            }
            catch (Exception lex)
            {
                throw lex;
            }
        }
    }
}
