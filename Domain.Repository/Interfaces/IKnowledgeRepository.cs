using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Core.Model;
using Developer;
using MongoDB.Bson;

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