using enterprises_test.Models.ViewModels;
using System.Threading.Tasks;

namespace enterprises_test.Services.Interfaces
{
    public interface IDepartmentEmployeeService
    {
        Task<PagedDataVMR<DepartmentsEmployee>> GetAll(int? size, int? pageNumber);
        Task<DepartmentsEmployee> GetById(long id);

        Task Post(DepartmentsEmployee item);
        Task Update(DepartmentsEmployee item);
        Task Delete(long id);
    }
}
