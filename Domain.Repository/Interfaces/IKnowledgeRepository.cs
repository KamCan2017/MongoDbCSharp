using Client.Core.Model;
using Core;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public interface IKnowledgeRepository
    {
        Task<bool> DeleteByIdAsync(string id);
        Task<bool> DeletedAsync(IKnowledge entity);
        Task<IEnumerable<KnowledgeModel>> FindAllAsync();
        Task<BsonDocument> InsertDocumentAsync(BsonDocument doc);
        Task<IKnowledge> SaveAsync(IKnowledge entity);
        Task<KnowledgeModel> UpdateAsync(KnowledgeModel entity);
        Task<bool> UpdateDocumentAsync(BsonDocument doc);
        Task<KnowledgeModel> FindByIdAsync(string id);
    }
}