using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Core.Model;
using MongoDB.Bson;

namespace Repository
{
    public interface IDeveloperRepository
    {
        BsonDocument CreateDocument(IDeveloper developer);
        Task<bool> DeleteAllAsync();
        Task<bool> DeleteByIdAsync(ObjectId id);
        Task<bool> DeletedAsync(IDeveloper entity);
        Task<IEnumerable<IDeveloper>> FindAllAsync();
        Task<IDeveloper> FindByIdAsync(ObjectId id);
        Task<IEnumerable<IDeveloper>> FindByTextSearch(string text);
        Task<IEnumerable<BsonDocument>> GetDocumentFromDeveloperView();
        Task<BsonDocument> InsertDocumentAsync(BsonDocument doc);
        Task<IDeveloper> SaveAsync(IDeveloper entity);
        Task<IDeveloper> UpdateAsync(IDeveloper entity);
    }
}