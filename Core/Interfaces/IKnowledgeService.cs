using MongoDB.Bson;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    [ServiceContract]
    public interface IKnowledgeService
    {
        [OperationContract]
        Task<bool> DeleteByIdAsync(string id);

        [OperationContract]
        Task<bool> DeletedAsync(KnowledgeModel entity);

        [OperationContract]
        Task<IEnumerable<KnowledgeModel>> FindAllAsync();

        [OperationContract]
        Task<BsonDocument> InsertDocumentAsync(BsonDocument doc);

        [OperationContract]
        Task<KnowledgeModel> SaveAsync(KnowledgeModel entity);

        [OperationContract]
        Task<KnowledgeModel> UpdateAsync(KnowledgeModel entity);

        [OperationContract]
        Task<bool> UpdateDocumentAsync(BsonDocument doc);

        [OperationContract]
        Task<KnowledgeModel> FindByIdAsync(string id);
    }
}
