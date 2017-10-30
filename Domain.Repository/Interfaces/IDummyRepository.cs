using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Repository
{
    public interface IDummyRepository
    {
        Task<bool> BuildIndexKeys();
        BsonDocument CreateDocument();
        Task<bool> DeleteAllDocumentAsync();
        Task<bool> DeleteDocumentAsync(ObjectId id);
        Task<IEnumerable<BsonDocument>> FindAllDocumentsAsync();
        Task<IEnumerable<BsonDocument>> FindDocumentByBalanaceGreaterThanAsync(int balance);
        Task<BsonDocument> FindDocumentByIdAsync(ObjectId id);
        Task<IEnumerable<BsonDocument>> FindDocumentsByTextSearch(string text);
        Task<IEnumerable<BsonDocument>> GroupDocumentByBalance();
        BsonDocument InsertDocument(BsonDocument doc);
        Task<BsonDocument> InsertDocumentAsync(BsonDocument doc);
        void UpdateDocument(BsonDocument document);
        Task UpdateDocumentAsync(BsonDocument document);
    }
}