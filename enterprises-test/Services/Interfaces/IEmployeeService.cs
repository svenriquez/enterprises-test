using enterprises_test.Models.ViewModels;
using System.Threading.Tasks;

namespace enterprises_test.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<PagedDataVMR<EmployeeVMR>> GetAll(int? size, int? pageNumber, string textFilter);
        Task<EmployeeVMR> GetById(long id);
        Task<EmployeeFormDataVMR> GetFormData(long? id);

        Task Post(Employee item);
        Task Update(Employee item);
        Task Delete(long id);
    }
}
