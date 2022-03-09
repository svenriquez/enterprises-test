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
    public class EnterpriseService : IEnterpriseService
    {
        readonly testContext context;

        public EnterpriseService(testContext _context)
        {
            context = _context;
        }

        public async Task<PagedDataVMR<Enterprise>> GetAll(int? pageSize, int? page, string textFilter)
        {
            PagedDataVMR<Enterprise> result = new PagedDataVMR<Enterprise>();

            try
            {
                var query = context.Enterprises.Select(x => new Enterprise() { 
                    Id = x.Id,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedDate,
                    Status = x.Status,
                    Address = x.Address,
                    Name = x.Name,
                    Phone = x.Phone
                });

                if (!String.IsNullOrWhiteSpace(textFilter))
                {
                    query = query.Where(x => x.Name.ToLower().Contains(textFilter.ToLower())
                    || x.Address.ToLower().Contains(textFilter.ToLower())
                    || x.Phone.ToLower().Contains(textFilter.ToLower()));
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

        public async Task<Enterprise> GetById(long id)
        {
            try
            {
                Enterprise enterprise = await context.Enterprises.Where(x => x.Id == id).Select(x => new Enterprise()
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
                }).FirstOrDefaultAsync();

                return enterprise;
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

        public async Task Post(Enterprise item)
        {
            try
            {
                item.CreatedDate = DateTime.Now;
                item.Status = true;

                context.Enterprises.Add(item);
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

        public async Task Update(Enterprise item)
        {
            try
            {
                Enterprise itemUpdate = context.Enterprises.Find(item.Id);

                itemUpdate.Name = item.Name;
                itemUpdate.Phone = item.Phone;
                itemUpdate.Status = item.Status;
                itemUpdate.Address = item.Address;
                itemUpdate.ModifiedBy = item.ModifiedBy;
                itemUpdate.ModifiedDate = DateTime.Now;

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
                Enterprise itemDelete = context.Enterprises.Find(id);

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
