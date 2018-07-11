using Core;
using Core.Interfaces;
using MongoDB.Bson;
using Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class DeveloperService : IDeveloperService
    {
        private readonly IDeveloperRepository _repository;

        public DeveloperService()
        {
            _repository = new DeveloperRepository();      
        }

        public async Task<DeveloperModel> CloneAsync(DeveloperModel entity)
        {
            return await _repository.CloneAsync(entity);
        }

        public BsonDocument CreateDocument(DeveloperModel developer)
        {
            return _repository.CreateDocument(developer);
        }

        public async Task<bool> DeleteAllAsync()
        {
            return await _repository.DeleteAllAsync();
        }

        public async Task<bool> DeleteAsync(DeveloperModel entity)
        {
            return await _repository.DeleteAsync(entity);
        }

        public async Task<bool> DeleteByIdAsync(ObjectId id)
        {
            return await _repository.DeleteByIdAsync(id);
        }

        public async Task<IEnumerable<DeveloperModel>> FindAllAsync()
        {
            return await _repository.FindAllAsync();
        }

        public async Task<DeveloperModel> FindByIdAsync(string id)
        {
            return await _repository.FindByIdAsync(id);
        }

        public async Task<IEnumerable<DeveloperModel>> FindByTextSearchAsync(string text)
        {
            return await _repository.FindByTextSearchAsync(text);
        }

        public async Task<IEnumerable<BsonDocument>> GetDocumentFromDeveloperViewAsync()
        {
            return await _repository.GetDocumentFromDeveloperView();
        }

        public async Task<BsonDocument> InsertDocumentAsync(BsonDocument doc)
        {
            return await _repository.InsertDocumentAsync(doc);
        }

        public async Task<DeveloperModel> SaveAsync(DeveloperModel entity)
        {
            return await _repository.SaveAsync(entity);
        }

        public async Task<IEnumerable<DeveloperModel>> SaveEntitiesAsync(IEnumerable<DeveloperModel> entities)
        {
            return await _repository.SaveAsync(entities);
        }

        public async Task<DeveloperModel> UpdateAsync(DeveloperModel entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<bool> UpdateDocumentAsync(BsonDocument doc)
        {
            return await _repository.UpdateDocumentAsync(doc);
        }
    }
}
