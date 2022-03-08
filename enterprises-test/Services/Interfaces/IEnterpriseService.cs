using enterprises_test.Models.ViewModels;
using System.Threading.Tasks;

namespace enterprises_test.Services.Interfaces
{
    public interface IEnterpriseService
    {
        Task<PagedDataVMR<Enterprise>> GetAll(int? size, int? pageNumber);
        Task<Enterprise> GetById(long id);

        Task Post(Enterprise item);
        Task Update(Enterprise item);
        Task Delete(long id);

    }
}
