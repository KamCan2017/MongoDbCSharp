using Client.Core.Model;
using Core;
using Core.Interfaces;
using MongoDB.Bson;
using Repository;
using System;
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

        public Task<IDeveloper> Clone(IDeveloper entity)
        {
            throw new NotImplementedException();
        }

        public BsonDocument CreateDocument(IDeveloper developer)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAll()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(IDeveloper entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteById(ObjectId id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DeveloperModel>> FindAll()
        {
            return await _repository.FindAllAsync();
        }

        public Task<IDeveloper> FindById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IDeveloper>> FindByTextSearch(string text)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BsonDocument>> GetDocumentFromDeveloperView()
        {
            throw new NotImplementedException();
        }

        public Task<BsonDocument> InsertDocument(BsonDocument doc)
        {
            throw new NotImplementedException();
        }

        public Task<IDeveloper> Save(IDeveloper entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IDeveloper>> SaveEntities(IEnumerable<IDeveloper> entities)
        {
            throw new NotImplementedException();
        }

        public Task<IDeveloper> Update(IDeveloper entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateDocument(BsonDocument doc)
        {
            throw new NotImplementedException();
        }
    }
}
