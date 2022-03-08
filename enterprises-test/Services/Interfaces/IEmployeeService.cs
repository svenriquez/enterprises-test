using enterprises_test.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace enterprises_test.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<PagedDataVMR<Employee>> GetAll(int? size, int? pageNumber);
        Task<Employee> GetById(long id);

        Task Post(Employee item);
        Task Update(Employee item);
        Task Delete(long id);
    }
}
