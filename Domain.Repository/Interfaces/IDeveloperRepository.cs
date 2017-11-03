using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Core.Model;
using MongoDB.Bson;
using Developer;

namespace Repository
{
    public interface IDeveloperRepository
    {
        BsonDocument CreateDocument(IDeveloper developer);
        Task<bool> DeleteAllAsync();
        Task<bool> DeleteByIdAsync(ObjectId id);
        Task<bool> DeletedAsync(IDeveloper entity);
        Task<IEnumerable<DeveloperModel>> FindAllAsync();
        Task<IDeveloper> FindByIdAsync(ObjectId id);
        Task<IEnumerable<DeveloperModel>> FindByTextSearch(string text);
        Task<IEnumerable<BsonDocument>> GetDocumentFromDeveloperView();
        Task<BsonDocument> InsertDocumentAsync(BsonDocument doc);
        Task<IDeveloper> SaveAsync(IDeveloper entity);
        Task<IDeveloper> UpdateAsync(IDeveloper entity);
    }
}