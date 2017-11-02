using System.Threading.Tasks;
using Client.Core.Model;

namespace Service
{
    public interface IDeveloperService
    {
        Task<bool> Save(IDeveloper entity);
    }
}