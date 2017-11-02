using Common;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Repository
{
    public static class MongoClientManager
    {
        private static MongoClient _client;
        static MongoClientManager()
        {
            #region credential
            //var credential = MongoCredential.CreateCredential("admin", "myUserAdmin", "abc123");
            ////Server settings
            //var settings = new MongoClientSettings
            //{
            //    Credentials = new[] { credential },
            //    Server = new MongoServerAddress("localhost"),
            //    UseSsl = false,           
            //};

            ////Get a Reference to the Client Object
            //_client = new MongoClient(settings);
            #endregion

            _client = new MongoClient("mongodb://localhost:27017");
            DataBase = _client.GetDatabase(DBNames.VDB);
            CreateViewInDb();
        }

        public static IMongoDatabase DataBase { get; private set; }


        private static void CreateViewInDb()
        {
            //setFeatureCompatibilityVersion for view 
            //BsonDocument setFeatureCompatibilityVersionCmd = new BsonDocument { { "setFeatureCompatibilityVersion", "3.4" } };
            //var doc = Task.Run(async () => await DataBase.RunCommandAsync<BsonDocument>(setFeatureCompatibilityVersionCmd)) ;

            var pipeline = PipelineDefinition<BsonDocument,BsonDocument>.Create(
               new BsonDocument {
                   { "$match",
                      new BsonDocument {{ "company_name", "cellent"}}
                   }
               });

           Task.Run(async () => 
            await DataBase.CreateViewAsync(CollectionNames.DeveloperView, CollectionNames.Developer, pipeline));
        }


        public static async Task CreateDummyDataBase()
        {
            var db = _client.GetDatabase(DBNames.Restaurant);
            await db.CreateCollectionAsync(CollectionNames.Employee);
            await db.CreateCollectionAsync(CollectionNames.Menu);
            await db.CreateCollectionAsync(CollectionNames.Dinner);

            Console.WriteLine("a new database {0} is created with collections {1}, {2}, {3}", DBNames.Restaurant,
                              CollectionNames.Employee, CollectionNames.Menu, CollectionNames.Dinner);
        }

        public static async Task DropDummyDataBase()
        {
            await _client.DropDatabaseAsync(DBNames.Restaurant);
            Console.WriteLine("{0} is deleted.", DBNames.Restaurant);
        }

    }
}
