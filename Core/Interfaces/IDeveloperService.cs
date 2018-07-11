using Client.Core.Model;
using MongoDB.Bson;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    [ServiceContract]
    public interface IDeveloperService
    {
        [OperationContract]
        BsonDocument CreateDocument(IDeveloper developer);

        [OperationContract]
        Task<bool> DeleteAll();

        [OperationContract]
        Task<bool> DeleteById(ObjectId id);

        [OperationContract]
        Task<bool> Delete(IDeveloper entity);

        [OperationContract]
        Task<IEnumerable<DeveloperModel>> FindAll();

        [OperationContract]
        Task<IDeveloper> FindById(string id);

        [OperationContract]
        Task<IEnumerable<IDeveloper>> FindByTextSearch(string text);

        [OperationContract]
        Task<IEnumerable<BsonDocument>> GetDocumentFromDeveloperView();

        [OperationContract]
        Task<BsonDocument> InsertDocument(BsonDocument doc);

        [OperationContract]
        Task<IDeveloper> Save(IDeveloper entity);

        [OperationContract]
        Task<IEnumerable<IDeveloper>> SaveEntities(IEnumerable<IDeveloper> entities);

        [OperationContract]
        Task<IDeveloper> Update(IDeveloper entity);

        [OperationContract]
        Task<bool> UpdateDocument(BsonDocument doc);

        [OperationContract]
        Task<IDeveloper> Clone(IDeveloper entity);
    }
}
