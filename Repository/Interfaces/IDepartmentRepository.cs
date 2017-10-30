using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Core.Model;

namespace Repository
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<IDepartment>> FindAllAsync();
        Task<IDepartment> SaveAsync(IDepartment entity);
    }
}