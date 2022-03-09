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
    public class EmployeeService : IEmployeeService
    {
        readonly testContext context;

        public EmployeeService(testContext _context)
        {
            context = _context;
        }

        public async Task<PagedDataVMR<EmployeeVMR>> GetAll(int? pageSize, int? page, string textFilter)
        {
            PagedDataVMR<EmployeeVMR> result = new PagedDataVMR<EmployeeVMR>();

            try
            {
                var query = context.Employees.Select(x => new EmployeeVMR()
                {
                    Id = x.Id,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedDate,
                    Status = x.Status,
                    Age = x.Age,
                    Email = x.Email,
                    Name = x.Name,
                    Position = x.Position,
                    Surname = x.Surname,
                    Department = x.DepartmentsEmployees.Where(x => x.Status == true).Select(x => x.IdDepartmentNavigation.Name).FirstOrDefault()
                });

                if (!String.IsNullOrWhiteSpace(textFilter))
                {
                    query = query.Where(x => x.Name.ToLower().Contains(textFilter.ToLower())
                    || x.Surname.ToLower().Contains(textFilter.ToLower())
                    || x.Email.ToLower().Contains(textFilter.ToLower())
                    || x.Position.ToLower().Contains(textFilter.ToLower())
                    || x.Department.Contains(textFilter.ToLower()));
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

        public async Task<EmployeeVMR> GetById(long id)
        {
            try
            {
                EmployeeVMR Employee = await context.Employees.Where(x => x.Id == id).Select(x => new EmployeeVMR()
                {
                    Id = x.Id,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedDate,
                    Status = x.Status,
                    Age = x.Age,
                    Email = x.Email,
                    Name = x.Name,
                    Position = x.Position,
                    Surname = x.Surname,
                    IdDepartment = x.DepartmentsEmployees.Where(x => x.Status == true).Select(x => x.IdDepartment).FirstOrDefault(),
                    IdEnterprise = x.DepartmentsEmployees.Select(x => x.IdDepartmentNavigation.IdEnterprise).FirstOrDefault()
                }).FirstOrDefaultAsync();

                return Employee;
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

        public async Task<EmployeeFormDataVMR> GetFormData(long? id)
        {
            try
            {
                EmployeeFormDataVMR resp = new EmployeeFormDataVMR();

                long enterprisesId = 0;
                if (id != null && id > 0)
                {
                    enterprisesId = context.DepartmentsEmployees.Where(x => x.IdEmployee == id).Select(x => x.IdDepartmentNavigation.IdEnterprise).FirstOrDefault();
                    resp.departmentList = context.Departments.Where(x => x.IdEnterprise == enterprisesId).Select(x => new Department()
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList();
                }

                resp.enterpriseList = await context.Enterprises.Where(x => x.Status == true).Select(x => new Enterprise()
                {
                    Id = x.Id,
                    Name = x.Name,
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

        public async Task Post(Employee item)
        {
            try
            {
                item.CreatedDate = DateTime.Now;
                item.Status = true;
                item.DepartmentsEmployees.ToList().ForEach(x => {
                    x.CreatedBy = item.CreatedBy;
                    x.CreatedDate = item.CreatedDate;
                    x.Status = true;
                });

                context.Employees.Add(item);
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

        public async Task Update(Employee item)
        {
            try
            {
                Employee itemUpdate = context.Employees.Find(item.Id);

                itemUpdate.ModifiedBy = item.ModifiedBy;
                itemUpdate.ModifiedDate = DateTime.Now;
                itemUpdate.Status = item.Status;

                itemUpdate.Age = item.Age;
                itemUpdate.Email = item.Email;
                itemUpdate.Name = item.Name;
                itemUpdate.Position = item.Position;
                itemUpdate.Surname = item.Surname;

                var departmentE = context.DepartmentsEmployees.Where(x => x.IdEmployee == item.Id).FirstOrDefault();
                if (departmentE != null)
                {
                    if (item.DepartmentsEmployees.FirstOrDefault().IdDepartment == -1)
                    {
                        departmentE.Status = false;
                    } else
                    {
                        departmentE.IdDepartment = item.DepartmentsEmployees.FirstOrDefault().IdDepartment;
                        departmentE.Status = true;
                    }

                    departmentE.ModifiedBy = item.ModifiedBy;
                    departmentE.ModifiedDate = DateTime.Now;
                    context.Entry(departmentE).State = EntityState.Modified;
                }

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
                Employee itemDelete = context.Employees.Find(id);

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
