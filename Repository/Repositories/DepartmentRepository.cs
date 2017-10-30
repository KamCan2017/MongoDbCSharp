using Client.Core.Model;
using Common;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        public async Task<IDepartment> SaveAsync(IDepartment entity)
        {
            var collection = MongoClientManager.DataBase.GetCollection<BsonDocument>(CollectionNames.Department);

            await collection.InsertOneAsync(entity.ToBsonDocument());
            Console.WriteLine("document added: " + entity.ToJson());

            var filter = new BsonDocument();

            Console.WriteLine("count:" + collection.Count(filter).ToString());


            return entity;
        }

        public async Task<IEnumerable<IDepartment>> FindAllAsync()
        {
            var collection = MongoClientManager.DataBase.GetCollection<IDepartment>(CollectionNames.Department);
            var filter = new BsonDocument();

            var docs = await collection.Find(filter).ToListAsync();
            Console.WriteLine("developers count: " + docs.Count);

            return docs;
        }
    }
}
