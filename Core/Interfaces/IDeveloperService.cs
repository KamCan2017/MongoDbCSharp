using Client.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IDeveloperService
    {
        Task<IEnumerable<IDeveloper>> FindAllAsync();
    }
}
