using Client.Core.Model;
using Developer;
using Common;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class DeveloperRepository : IDeveloperRepository
    {
        static DeveloperRepository()
        {
            BuildIndexKeys();
        }

        private static bool BuildIndexKeys()
        {
            var collection = MongoClientManager.DataBase.GetCollection<BsonDocument>(CollectionNames.Developer);

            //Text index for all fields
            var keys = Builders<BsonDocument>.IndexKeys.Text("$**");
            var result = collection.Indexes.CreateOne(keys);

            return !string.IsNullOrEmpty(result);
        }
        public BsonDocument CreateDocument(IDeveloper developer)
        {
            if (developer == null)
                return null;

            var doc = developer.ToBsonDocument();
            return doc;
        }

        public async Task<IDeveloper> SaveAsync(IDeveloper entity)
        {
            var collection = MongoClientManager.DataBase.GetCollection<DeveloperModel>(CollectionNames.Developer);

            await collection.InsertOneAsync(entity as DeveloperModel);
            Console.WriteLine("document added: " + entity.ToJson());

            var filter = new BsonDocument();

            Console.WriteLine("count:" + collection.Count(filter).ToString());


            return entity;
        }

        public async Task<DeveloperModel> UpdateAsync(DeveloperModel entity)
        {
            var collection = MongoClientManager.DataBase.GetCollection<DeveloperModel>(CollectionNames.Developer);

            //copy the array list to update separatly the array list
            var array = entity.KnowledgeBase.ToList();
            entity.KnowledgeBase = null;

            await collection.ReplaceOneAsync(d => d.ID == entity.ID, entity);

            //update separatly the array list
            var update = Builders<DeveloperModel>.Update.Set(p => p.KnowledgeBase, array);
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

            var docs = await collection.Find(filter).ToListAsync();
            Console.WriteLine("developers count: " + docs.Count);

            return docs;
        }

        public async Task<IDeveloper> FindByIdAsync(ObjectId id)
        {
            var collection = MongoClientManager.DataBase.GetCollection<DeveloperModel>(CollectionNames.Developer);

            var result = await collection.FindAsync(d => d.ID == id);
            return result.FirstOrDefault();
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
            return await collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<BsonDocument>> GetDocumentFromDeveloperView()
        {
            var collection = MongoClientManager.DataBase.GetCollection<BsonDocument>(CollectionNames.DeveloperView);
            var filter = new BsonDocument();

            var docs = await collection.Find(filter).ToListAsync();
            return docs;
        }
    }
}
