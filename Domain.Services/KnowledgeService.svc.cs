using Core;
using Core.Interfaces;
using MongoDB.Bson;
using Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class KnowledgeService : IKnowledgeService
    {
        private readonly IKnowledgeRepository _repository;

        public KnowledgeService()
        {
            _repository = new KnowledgeRepository();
        }

        public async Task<bool> DeleteByIdAsync(string id)
        {
            return await _repository.DeleteByIdAsync(id);
        }

        public async Task<bool> DeletedAsync(KnowledgeModel entity)
        {
            return await _repository.DeletedAsync(entity);
        }

        public async Task<IEnumerable<KnowledgeModel>> FindAllAsync()
        {
            return await _repository.FindAllAsync();
        }

        public async Task<KnowledgeModel> FindByIdAsync(string id)
        {
            return await _repository.FindByIdAsync(id);
        }

        public async Task<BsonDocument> InsertDocumentAsync(BsonDocument doc)
        {
            return await _repository.InsertDocumentAsync(doc);
        }

        public async Task<KnowledgeModel> SaveAsync(KnowledgeModel entity)
        {
            return await _repository.SaveAsync(entity);
        }

        public async Task<KnowledgeModel> UpdateAsync(KnowledgeModel entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<bool> UpdateDocumentAsync(BsonDocument doc)
        {
            return await _repository.UpdateDocumentAsync(doc);
        }
    }
}
