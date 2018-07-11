using Client.Core.Model;
using Developer;
using Common;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;

namespace Repository
{
    public class KnowledgeRepository : IKnowledgeRepository
    {
       

        public async Task<IKnowledge> SaveAsync(IKnowledge entity)
        {
            var collection = MongoClientManager.DataBase.GetCollection<KnowledgeModel>(CollectionNames.Knowledge);

            await collection.InsertOneAsync(entity as KnowledgeModel);
            Console.WriteLine("document added: " + entity.ToJson());

            var filter = new BsonDocument();

            Console.WriteLine("count:" + collection.Count(filter).ToString());


            return entity;
        }

        public async Task<KnowledgeModel> UpdateAsync(KnowledgeModel entity)
        {
            var collection = MongoClientManager.DataBase.GetCollection<KnowledgeModel>(CollectionNames.Knowledge);


            await collection.ReplaceOneAsync(d => d.ID == entity.ID, entity);

            Console.WriteLine("document updated: " + entity.ToJson());

            var filter = new BsonDocument();
            Console.WriteLine("count:" + collection.CountDocuments(filter).ToString());


            return entity;
        }

        public async Task<bool> UpdateDocumentAsync(BsonDocument doc)
        {
            var collection = MongoClientManager.DataBase.GetCollection<BsonDocument>(CollectionNames.Knowledge);

            var filter1 = Builders<BsonDocument>.Filter.Eq("_id", doc["_id"]);
            await collection.ReplaceOneAsync(filter1, doc);
            Console.WriteLine("document updated: " + doc.ToJson());

            var filter = new BsonDocument();
            Console.WriteLine("count:" + collection.Count(filter).ToString());


            return true;
        }

        public async Task<BsonDocument> InsertDocumentAsync(BsonDocument doc)
        {
            var collection = MongoClientManager.DataBase.GetCollection<BsonDocument>(CollectionNames.Knowledge);

            await collection.InsertOneAsync(doc);
            Console.WriteLine("document added: " + doc.ToJson());

            var filter = new BsonDocument();

            Console.WriteLine("count:" + collection.Count(filter).ToString());

            return doc;
        }
      

        public async Task<IEnumerable<KnowledgeModel>> FindAllAsync()
        {
            var collection = MongoClientManager.DataBase.GetCollection<KnowledgeModel>(CollectionNames.Knowledge);
            var filter = new BsonDocument();

            var docs = await collection.Find(filter).ToListAsync();
            Console.WriteLine("developers count: " + docs.Count);

            return docs;
        }

        public async Task<KnowledgeModel> FindByIdAsync(string id)
        {
            var collection = MongoClientManager.DataBase.GetCollection<KnowledgeModel>(CollectionNames.Knowledge);

            var result = await collection.FindAsync(d => d.ID == id);
            return result.FirstOrDefault();
        }



        public async Task<bool> DeleteByIdAsync(string id)
        {
            var collection = MongoClientManager.DataBase.GetCollection<KnowledgeModel>(CollectionNames.Knowledge);

            var result = await collection.DeleteOneAsync(d => d.ID == id);

            //check the document count
            var docs = await collection.Find(new BsonDocument()).ToListAsync();
            Console.WriteLine("document count: " + docs.Count);

            return result.DeletedCount == 1;
        }

        public async Task<bool> DeletedAsync(IKnowledge entity)
        {
            var collection = MongoClientManager.DataBase.GetCollection<KnowledgeModel>(CollectionNames.Knowledge);

            var result = await collection.DeleteOneAsync(d => d.ID == entity.ID);

            //check the document count
            var docs = await collection.Find(new BsonDocument()).ToListAsync();
            Console.WriteLine("document count: " + docs.Count);

            return result.DeletedCount == 1;
        }   
       
    }
}
