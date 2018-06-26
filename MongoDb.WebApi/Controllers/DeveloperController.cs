using Client.Core.Model;
using Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace MongoDb.WebApi.Controllers
{
    public class DeveloperController : ApiController
    {
        private IDeveloperRepository _developerRepository;

        public DeveloperController()
        {
            // var repository = ServiceLocator.Current.GetInstance<IDeveloperRepository>();
            //_developerRepository = new DeveloperRepository();
        }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            _developerRepository = new DeveloperRepository();
        }


        [HttpGet]
        public async Task<IEnumerable<IDeveloper>> Get()
        {
            return await _developerRepository.FindAllAsync();
        }
    }
}
