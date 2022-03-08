using enterprises_test.Models.ViewModels;
using System.Threading.Tasks;

namespace enterprises_test.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<PagedDataVMR<Department>> GetAll(int? size, int? pageNumber);
        Task<Department> GetById(long id);

        Task Post(Department item);
        Task Update(Department item);
        Task Delete(long id);
    }
}
