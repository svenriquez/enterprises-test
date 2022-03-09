using enterprises_test.Models;
using enterprises_test.Models.ViewModels;
using enterprises_test.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace enterprises_test.Services
{
    public class DepartmentService : IDepartmentService
    {
        readonly testContext context;

        public DepartmentService(testContext _context)
        {
            context = _context;
        }

        public async Task<PagedDataVMR<Department>> GetAll(int? pageSize, int? page, string textFilter)
        {
            PagedDataVMR<Department> result = new PagedDataVMR<Department>();

            try
            {
                var query = context.Departments.Select(x => new Department()
                {
                    Id = x.Id,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedDate,
                    Status = x.Status,
                    Description = x.Description,
                    Name = x.Name,
                    Phone = x.Phone,
                    IdEnterpriseNavigation = new Enterprise()
                    {
                        Id = x.IdEnterpriseNavigation.Id,
                        Name = x.IdEnterpriseNavigation.Name
                    }
                });

                if (!String.IsNullOrWhiteSpace(textFilter))
                {
                    query = query.Where(x => x.Name.ToLower().Contains(textFilter.ToLower())
                    || x.Description.ToLower().Contains(textFilter.ToLower())
                    || x.Phone.ToLower().Contains(textFilter.ToLower())
                    || x.IdEnterpriseNavigation.Name.ToLower().Contains(textFilter.ToLower()));
                }

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

        public async Task<Department> GetById(long id)
        {
            try
            {
                Department Department = await context.Departments.Where(x => x.Id == id).Select(x => new Department()
                {
                    Id = x.Id,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedDate,
                    Status = x.Status,
                    Description = x.Description,
                    Name = x.Name,
                    Phone = x.Phone,
                    IdEnterprise = x.IdEnterprise,
                    IdEnterpriseNavigation = new Enterprise()
                    {
                        Id = x.IdEnterpriseNavigation.Id,
                        Name = x.IdEnterpriseNavigation.Name
                    }
                }).FirstOrDefaultAsync();

                return Department;
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

        public async Task<DepartmentFormDataVMR> GetFormData(long? id)
        {
            try
            {
                long enterprisesIdList = 0;
                if (id != null && id > 0)
                {
                    enterprisesIdList = context.Departments.Where(x => x.Id == id).Select(x => x.IdEnterprise).FirstOrDefault();
                }

                DepartmentFormDataVMR resp = new DepartmentFormDataVMR();
                resp.enterpriseList = await context.Enterprises.Where(x => x.Status == true || enterprisesIdList == x.Id).Select(x => new Enterprise()
                {
                    Id = x.Id,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedDate,
                    Status = x.Status,
                    Address = x.Address,
                    Name = x.Name,
                    Phone = x.Phone
                }).ToListAsync();

                return resp;
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

        public async Task<List<Department>> GetDepartmentsByIdEnterprise(long id)
        {
            List<Department> result = new List<Department>();

            try
            {
                result = await context.Departments.Where(x => x.IdEnterprise == id && x.Status == true).Select(x => new Department()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Phone = x.Phone
                }).ToListAsync();

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

        public async Task Post(Department item)
        {
            try
            {
                item.CreatedDate = DateTime.Now;
                item.Status = true;

                context.Departments.Add(item);
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

        public async Task Update(Department item)
        {
            try
            {
                Department itemUpdate = context.Departments.Find(item.Id);

                itemUpdate.ModifiedBy = item.ModifiedBy;
                itemUpdate.ModifiedDate = DateTime.Now;
                itemUpdate.Status = item.Status;

                itemUpdate.Description = item.Description;
                itemUpdate.Name = item.Name;
                itemUpdate.Phone = item.Phone;
                itemUpdate.IdEnterprise = item.IdEnterprise;

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
                Department itemDelete = context.Departments.Find(id);

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
