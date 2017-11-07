using Common;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using System.Linq;
using MongoDB.Driver.Core.Clusters;

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
            CheckServerConnection();
            CreateViewInDb();
            BuildIndexKeys();
        }

        public static IMongoDatabase DataBase { get; private set; }

        private static void CheckServerConnection()
        {
            bool isMongoLive = DataBase.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(2000);
            if (isMongoLive)
            {
                // connected
            }
            else
            {
                // couldn't connect
                throw new Exception("Server connection failed!");
            }

        }
        private static void CreateViewInDb()
        {
            try
            {
                Task.Run(async () => 
                {
                    var filter = new BsonDocument("name", CollectionNames.DeveloperView);
                    //filter by collection name
                    var collections = await DataBase.ListCollectionsAsync(new ListCollectionsOptions { Filter = filter });
                    //check for existence
                    var viewExist = await collections.AnyAsync();

                    if(!viewExist)
                    {
                        //setFeatureCompatibilityVersion for view 
                        BsonDocument setFeatureCompatibilityVersionCmd = new BsonDocument { { "setFeatureCompatibilityVersion", "3.4" } };
                        await _client.GetDatabase(DBNames.admin).RunCommandAsync<BsonDocument>(setFeatureCompatibilityVersionCmd);

                        var pipeline = PipelineDefinition<BsonDocument, BsonDocument>.Create(
                           new BsonDocument {
                                               { "$match",
                                                  new BsonDocument {{ "company_name", "cellent"}}
                                               }
                           });
                        await DataBase.CreateViewAsync(CollectionNames.DeveloperView, CollectionNames.Developer, pipeline);
                    }
                });


            }
            catch (Exception)
            {

                throw;
            }

        }

        private static bool BuildIndexKeys()
        {
            var collection = DataBase.GetCollection<BsonDocument>(CollectionNames.Developer);

            //Text index for all fields
            var keys = Builders<BsonDocument>.IndexKeys.Text("$**");
            var result = collection.Indexes.CreateOne(keys);

            return !string.IsNullOrEmpty(result);
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
