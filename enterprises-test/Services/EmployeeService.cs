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

        public async Task<PagedDataVMR<Employee>> GetAll(int? pageSize, int? page)
        {
            PagedDataVMR<Employee> result = new PagedDataVMR<Employee>();

            try
            {
                var query = context.Employees.Select(x => new Employee()
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
                    Surname = x.Surname
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

        public async Task<Employee> GetById(long id)
        {
            try
            {
                Employee Employee = await context.Employees.Where(x => x.Id == id).Select(x => new Employee()
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
                    Surname = x.Surname
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

        public async Task Post(Employee item)
        {
            try
            {
                item.CreatedDate = DateTime.Now;
                item.Status = true;

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
