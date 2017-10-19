using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbConsole.Helper;
using MongoDbConsole.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDbConsole.Repositories
{
    public class DepartmentRepository
    {
        public async Task<Department> SaveAsync(Department entity)
        {
            var collection = MongoClientManager.DataBase.GetCollection<BsonDocument>(CollectionNames.Department);

            await collection.InsertOneAsync(entity.ToBsonDocument());
            Console.WriteLine("document added: " + entity.ToJson());

            var filter = new BsonDocument();

            Console.WriteLine("count:" + collection.Count(filter).ToString());


            return entity;
        }

        public async Task<IEnumerable<Department>> FindAllAsync()
        {
            var collection = MongoClientManager.DataBase.GetCollection<Department>(CollectionNames.Department);
            var filter = new BsonDocument();

            var docs = await collection.Find(filter).ToListAsync();
            Console.WriteLine("developers count: " + docs.Count);

            return docs;
        }
    }
}
