using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Core.Model;
using MongoDB.Bson;
using Developer;
using Core;

namespace Repository
{
    public interface IDeveloperRepository
    {
        BsonDocument CreateDocument(DeveloperModel developer);

        Task<bool> DeleteAllAsync();

        Task<bool> DeleteByIdAsync(ObjectId id);

        Task<bool> DeleteAsync(IDeveloper entity);

        Task<IEnumerable<DeveloperModel>> FindAllAsync();

        Task<DeveloperModel> FindByIdAsync(string id);

        Task<IEnumerable<DeveloperModel>> FindByTextSearchAsync(string text);

        Task<IEnumerable<BsonDocument>> GetDocumentFromDeveloperView();

        Task<BsonDocument> InsertDocumentAsync(BsonDocument doc);

        Task<DeveloperModel> SaveAsync(DeveloperModel entity);

        Task<IEnumerable<DeveloperModel>> SaveAsync(IEnumerable<DeveloperModel> entities);

        Task<DeveloperModel> UpdateAsync(DeveloperModel entity);

        Task<bool> UpdateDocumentAsync(BsonDocument doc);

        Task<DeveloperModel> CloneAsync(DeveloperModel entity);
    }
}