using Common;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class DummyRepository : IDummyRepository
    {
        public async Task<bool> BuildIndexKeys()
        {
            //index for the field lastname
            var keys = Builders<BsonDocument>.IndexKeys.Ascending("lastname");

            var collection = MongoClientManager.DataBase.GetCollection<BsonDocument>(CollectionNames.Dummys);
            var result1 = await collection.Indexes.CreateOneAsync(keys);

            //index for the array accounts
            keys = Builders<BsonDocument>.IndexKeys.Ascending("accounts");
            var result2 = await collection.Indexes.CreateOneAsync(keys);

            //Text index for all fields
            keys = Builders<BsonDocument>.IndexKeys.Text("$**");
            var result3 = await collection.Indexes.CreateOneAsync(keys);

            return !string.IsNullOrEmpty(result1) && !string.IsNullOrEmpty(result2)
                   && !string.IsNullOrEmpty(result3);
        }

        public BsonDocument CreateDocument()
        {
            return new BsonDocument
            {
                {"firstname", "asty"},
                {"lastname", "noukeu"},
                {"accounts", new BsonArray
                {
                    new BsonDocument
                    {
                        {"account_balance", 5000 },
                         {"account_type", "Investment" },
                         {"currency", "USD" }
                    }
                }
               }
            };
        }

        public BsonDocument InsertDocument(BsonDocument doc)
        {
            var collection = MongoClientManager.DataBase.GetCollection<BsonDocument>(CollectionNames.Dummys);           

            collection.InsertOne(doc);
            Console.WriteLine("document added: " + doc.ToJson());

            var filter = new BsonDocument();

            Console.WriteLine("count:" + collection.Count(filter).ToString());

            return doc;
        }

        public async Task<BsonDocument> InsertDocumentAsync(BsonDocument doc)
        {
            var collection = MongoClientManager.DataBase.GetCollection<BsonDocument>(CollectionNames.Dummys);

            await collection.InsertOneAsync(doc);
            Console.WriteLine("document added: " + doc.ToJson());

            var filter = new BsonDocument();

            Console.WriteLine("count:" + collection.Count(filter).ToString());


            return doc;
        }


        public void UpdateDocument(BsonDocument document)
        {
            var collection = MongoClientManager.DataBase.GetCollection<BsonDocument>(CollectionNames.Dummys);
            var newValue = document["accounts"][0]["account_balance"].AsInt32 + 1000;

            var filter1 = Builders<BsonDocument>.Filter.Eq("_id", document["_id"]);
            var update = Builders<BsonDocument>.Update.Set("accounts.0.account_balance", newValue);
            var result = collection.UpdateOne(filter1, update);
            Console.WriteLine("document updated");
        }

        public async Task UpdateDocumentAsync(BsonDocument document)
        {           
            var collection = MongoClientManager.DataBase.GetCollection<BsonDocument>(CollectionNames.Dummys);
            var newValue = document["accounts"][0]["account_balance"].AsInt32 + 1000;

            var filter1 = Builders<BsonDocument>.Filter.Eq("_id", document["_id"]);
            var update = Builders<BsonDocument>.Update.Set("accounts.0.account_balance", newValue);

            var result = await collection.UpdateOneAsync(filter1, update);
            //if (result.IsModifiedCountAvailable)
            //{
            //    var test = result.ModifiedCount.Should().Be(1);
            //}
            Console.WriteLine("document updated id " + document["_id"]);
        }

        public async Task<IEnumerable<BsonDocument>> FindAllDocumentsAsync()
        {
            var collection = MongoClientManager.DataBase.GetCollection<BsonDocument>(CollectionNames.Dummys);
            var filter = new BsonDocument();

            var docs = await collection.Find(filter).ToListAsync();
            Console.WriteLine("documents count: " + docs.Count);

            return docs;
        }

        public async Task<BsonDocument> FindDocumentByIdAsync(ObjectId id)
        {
            var collection = MongoClientManager.DataBase.GetCollection<BsonDocument>(CollectionNames.Dummys);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);

            //1.
            var result = await collection.FindAsync(filter);
            var doc = result.FirstOrDefault();
            int count = doc != null ? 1 : 0;
            Console.WriteLine("{0} document found", count);

            return doc;

            ////2.
            //var result = await collection.Find(filter).ToListAsync();
            //return result.FirstOrDefault();
        }

        public async Task<IEnumerable<BsonDocument>> FindDocumentByBalanaceGreaterThanAsync(int balance)
        {
            var collection = MongoClientManager.DataBase.GetCollection<BsonDocument>(CollectionNames.Dummys);
            var filter = Builders<BsonDocument>.Filter.Gt("accounts.0.account_balance", balance);
             //var filter = Builders<BsonDocument>.Filter.Lt("accounts.0.account_balance", balance);
            var result = await collection.Find(filter).ToListAsync();

            Console.WriteLine("{0} document found", result.Count);

            return result;
        }

        public async Task<bool> DeleteDocumentAsync(ObjectId id)
        {
            var collection = MongoClientManager.DataBase.GetCollection<BsonDocument>(CollectionNames.Dummys);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);

            var result = await collection.DeleteOneAsync(filter);

            //check the document count
            var docs = await collection.Find(new BsonDocument()).ToListAsync();
            Console.WriteLine("document count: " + docs.Count);

            return result.DeletedCount == 1;
        }

        public async Task<bool> DeleteAllDocumentAsync()
        {
            var collection = MongoClientManager.DataBase.GetCollection<BsonDocument>(CollectionNames.Dummys);
            var filter = new BsonDocument();
            var docsToDelete = collection.Count(filter);
            var result = await collection.DeleteManyAsync(filter);

            //check the document count
            var docs = await collection.Find(new BsonDocument()).ToListAsync();
            Console.WriteLine("document count: " + docs.Count);

            return result.DeletedCount == docsToDelete;
        }


        //Aggregation opperations, operations, such as grouping by a specified key
        //and evaluating a total or a count for each distinct group.
        public async Task<IEnumerable<BsonDocument>> GroupDocumentByBalance()
        {
            var collection = MongoClientManager.DataBase.GetCollection<BsonDocument>(CollectionNames.Dummys);
            var aggregate = collection.Aggregate().Group(new BsonDocument { { "_id", "$accounts.account_balance" }, { "count", new BsonDocument("$sum", 1) } });
            var results = await aggregate.ToListAsync();

            return results;
        }

        public async Task<IEnumerable<BsonDocument>> FindDocumentsByTextSearch(string text)
        {
            var collection = MongoClientManager.DataBase.GetCollection<BsonDocument>(CollectionNames.Dummys);

            var filter = Builders<BsonDocument>.Filter.Text(text);
            return await collection.Find(filter).ToListAsync();
        }
       
    }
}
