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
        BsonDocument CreateDocument(DeveloperModel developer);

        [OperationContract]
        Task<bool> DeleteAllAsync();

        [OperationContract]
        Task<bool> DeleteByIdAsync(ObjectId id);

        [OperationContract]
        Task<bool> DeleteAsync(DeveloperModel entity);

        [OperationContract]
        Task<IEnumerable<DeveloperModel>> FindAllAsync();

        [OperationContract]
        Task<DeveloperModel> FindByIdAsync(string id);

        [OperationContract]
        Task<IEnumerable<DeveloperModel>> FindByTextSearchAsync(string text);

        [OperationContract]
        Task<IEnumerable<BsonDocument>> GetDocumentFromDeveloperViewAsync();

        [OperationContract]
        Task<BsonDocument> InsertDocumentAsync(BsonDocument doc);

        [OperationContract]
        Task<DeveloperModel> SaveAsync(DeveloperModel entity);

        [OperationContract]
        Task<IEnumerable<DeveloperModel>> SaveEntitiesAsync(IEnumerable<DeveloperModel> entities);

        [OperationContract]
        Task<DeveloperModel> UpdateAsync(DeveloperModel entity);

        [OperationContract]
        Task<bool> UpdateDocumentAsync(BsonDocument doc);

        [OperationContract]
        Task<DeveloperModel> CloneAsync(DeveloperModel entity);
    }
}
