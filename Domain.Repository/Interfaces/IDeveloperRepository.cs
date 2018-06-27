using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Core.Model;
using MongoDB.Bson;
using Developer;

namespace Repository
{
    public interface IDeveloperRepository
    {
        BsonDocument CreateDocument(DeveloperModel developer);

        Task<bool> DeleteAllAsync();

        Task<bool> DeleteByIdAsync(ObjectId id);

        Task<bool> DeleteAsync(IDeveloper entity);

        Task<IEnumerable<IDeveloper>> FindAllAsync();

        Task<DeveloperModel> FindByIdAsync(ObjectId id);

        Task<IEnumerable<DeveloperModel>> FindByTextSearch(string text);

        Task<IEnumerable<BsonDocument>> GetDocumentFromDeveloperView();

        Task<BsonDocument> InsertDocumentAsync(BsonDocument doc);

        Task<IDeveloper> SaveAsync(IDeveloper entity);

        Task<IEnumerable<DeveloperModel>> SaveAsync(IEnumerable<DeveloperModel> entities);

        Task<DeveloperModel> UpdateAsync(DeveloperModel entity);

        Task<bool> UpdateDocumentAsync(BsonDocument doc);

        Task<DeveloperModel> CloneAsync(DeveloperModel entity);
    }
}