using Client.Core.Model;
using Developer;
using Common;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Repository
{
    public class DeveloperRepository : IDeveloperRepository
    {
       
        public BsonDocument CreateDocument(DeveloperModel developer)
        {
            if (developer == null)
                return null;

            var doc = developer.ToBsonDocument();
            return doc;
        }

        public async Task<IDeveloper> SaveAsync(IDeveloper entity)
        {
            var collection = MongoClientManager.DataBase.GetCollection<DeveloperModel>(CollectionNames.Developer);
            var model = entity as DeveloperModel;
            if (model.KnowledgeBase != null && model.KnowledgeBase.Any())
            {
                model.KnowledgeIds = model.KnowledgeBase.Select(p => p.ID).ToList();
            }
            model.KnowledgeBase = null;

            await collection.InsertOneAsync(model);
            Console.WriteLine("document added: " + entity.ToJson());

            var filter = new BsonDocument();

            Console.WriteLine("count:" + collection.Count(filter).ToString());


            return entity;
        }

        public async Task<DeveloperModel> UpdateAsync(DeveloperModel entity)
        {
            var collection = MongoClientManager.DataBase.GetCollection<DeveloperModel>(CollectionNames.Developer);

            //copy the array list to update separatly the array list
            var array = new List<ObjectId>();
            if (entity.KnowledgeBase != null && entity.KnowledgeBase.Any())
            {
                array = entity.KnowledgeBase.Select(p => p.ID).ToList();
            }
            entity.KnowledgeIds = null;
            entity.KnowledgeBase = null;

            await collection.ReplaceOneAsync(d => d.ID == entity.ID, entity);

            //update separatly the array list
            var update = Builders<DeveloperModel>.Update.Set(p => p.KnowledgeIds, array);
            await collection.UpdateOneAsync(d => d.ID == entity.ID, update);

            Console.WriteLine("document updated: " + entity.ToJson());

            var filter = new BsonDocument();
            Console.WriteLine("count:" + collection.Count(filter).ToString());


            return entity;
        }

        public async Task<bool> UpdateDocumentAsync(BsonDocument doc)
        {
            var collection = MongoClientManager.DataBase.GetCollection<BsonDocument>(CollectionNames.Developer);

            var filter1 = Builders<BsonDocument>.Filter.Eq("_id", doc["_id"]);
            await collection.ReplaceOneAsync(filter1, doc);
            Console.WriteLine("document updated: " + doc.ToJson());

            var filter = new BsonDocument();
            Console.WriteLine("count:" + collection.Count(filter).ToString());


            return true;
        }

        public async Task<BsonDocument> InsertDocumentAsync(BsonDocument doc)
        {
            var collection = MongoClientManager.DataBase.GetCollection<BsonDocument>(CollectionNames.Developer);

            await collection.InsertOneAsync(doc);
            Console.WriteLine("document added: " + doc.ToJson());

            var filter = new BsonDocument();

            Console.WriteLine("count:" + collection.Count(filter).ToString());

            return doc;
        }
      

        public async Task<IEnumerable<DeveloperModel>> FindAllAsync()
        {
            var collection = MongoClientManager.DataBase.GetCollection<DeveloperModel>(CollectionNames.Developer);
            var filter = new BsonDocument();

            var entities = await collection.Find(filter).ToListAsync();
            Console.WriteLine("developers count: " + entities.Count);
            if(entities.Any())
            {
                foreach(var entity in entities)
                {
                  await FillKnowledge(entity);
                }
            }
            return entities;
        }

        public async Task<DeveloperModel> FindByIdAsync(ObjectId id)
        {
            var collection = MongoClientManager.DataBase.GetCollection<DeveloperModel>(CollectionNames.Developer);

            var result = await collection.FindAsync(d => d.ID == id);
            var entity = result.FirstOrDefault();
            if (entity != null)
                await FillKnowledge(entity);

            return entity;
        }


        public async Task<bool> DeleteByIdAsync(ObjectId id)
        {
            var collection = MongoClientManager.DataBase.GetCollection<DeveloperModel>(CollectionNames.Developer);

            var result = await collection.DeleteOneAsync(d => d.ID == id);

            //check the document count
            var docs = await collection.Find(new BsonDocument()).ToListAsync();
            Console.WriteLine("document count: " + docs.Count);

            return result.DeletedCount == 1;
        }

        public async Task<bool> DeletedAsync(IDeveloper entity)
        {
            var collection = MongoClientManager.DataBase.GetCollection<DeveloperModel>(CollectionNames.Developer);

            var result = await collection.DeleteOneAsync(d => d.ID == entity.ID);

            //check the document count
            var docs = await collection.Find(new BsonDocument()).ToListAsync();
            Console.WriteLine("document count: " + docs.Count);

            return result.DeletedCount == 1;
        }

        public async Task<bool> DeleteAllAsync()
        {
            var collection = MongoClientManager.DataBase.GetCollection<DeveloperModel>(CollectionNames.Developer);
            var filter = new BsonDocument();
            var docsToDelete = collection.Count(filter);
            var result = await collection.DeleteManyAsync(filter);

            //check the document count
            var docs = await collection.Find(new BsonDocument()).ToListAsync();
            Console.WriteLine("document count: " + docs.Count);

            return result.DeletedCount == docsToDelete;
        }


        public async Task<IEnumerable<DeveloperModel>> FindByTextSearch(string text)
        {
            if (string.IsNullOrEmpty(text))
                return new List<DeveloperModel>();

            var collection = MongoClientManager.DataBase.GetCollection<DeveloperModel>(CollectionNames.Developer);

            var filter = Builders<DeveloperModel>.Filter.Text(text);
            var entities = await collection.Find(filter).ToListAsync();
            if (entities.Any())
            {
                foreach (var entity in entities)
                {
                    await FillKnowledge(entity);
                }
            }
            return entities;
        }

        public async Task<IEnumerable<BsonDocument>> GetDocumentFromDeveloperView()
        {
            var collection = MongoClientManager.DataBase.GetCollection<BsonDocument>(CollectionNames.DeveloperView);
            var filter = new BsonDocument();

            var docs = await collection.Find(filter).ToListAsync();
            return docs;
        }

        private async Task FillKnowledge(DeveloperModel entity)
        {
            if(entity.KnowledgeIds.Any())
            {
                var collection = MongoClientManager.DataBase.GetCollection<KnowledgeModel>(CollectionNames.Knowledge);
                var filter = Builders<KnowledgeModel>.Filter.AnyIn("_id", entity.KnowledgeIds.ToArray());
                var doc = filter.ToBsonDocument();
                var entities = await collection.Find(filter).ToListAsync();
                entity.KnowledgeBase = new ObservableCollection<KnowledgeModel>(entities);
            }
        }
    }
}
