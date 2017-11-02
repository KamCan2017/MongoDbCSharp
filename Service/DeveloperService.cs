using Client.Core.Model;
using Repository;
using System.Threading.Tasks;

namespace Service
{
    public class DeveloperService : IDeveloperService
    {
        private DeveloperRepository _developerRepository = new DeveloperRepository();

        public async Task<bool> Save(IDeveloper entity)
        {
            await _developerRepository.SaveAsync(entity);
            return true;
        }
    }
}
