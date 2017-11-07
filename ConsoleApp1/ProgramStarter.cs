using Client.Core.Model;
using Developer;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Repository;
using System;
using System.Threading.Tasks;

namespace MongoDbConsole
{
    /// <summary>
    /// Class used to make requests on the MongoDB 
    /// </summary>
    public static class ProgramStarter
    {
        /// <summary>
        /// Make some requests on the Developer collection
        /// </summary>
        public static void DeveloperCollectionTest()
        {
            IDeveloperRepository repository = new DeveloperRepository();
            var developer = Factory.CreateDeveloper();
            BsonDocument doc = repository.CreateDocument(developer);

            //Insert a document
            var savedDoc = Task.Factory.StartNew(async () => await repository.InsertDocumentAsync(doc))
                .GetAwaiter().GetResult().Result;

            //Update a document
            DeveloperModel savedObj = BsonSerializer.Deserialize<DeveloperModel>(savedDoc);
            savedObj.CompanyName = "Bosch";

            savedObj = Task.Factory.StartNew(async () => await repository.UpdateAsync(savedObj))
                .GetAwaiter().GetResult().Result;

            //Find all developers
            var developers = Task.Factory.StartNew(async () => await repository.FindAllAsync())
               .GetAwaiter().GetResult().Result;

            //Find by id
            var obj = Task.Factory.StartNew(async () => await repository.FindByIdAsync(savedObj.ID))
                .GetAwaiter().GetResult().Result;

            if (obj != null)
                Console.WriteLine(obj.ToJson());

            //Find by text

            var objects = Task.Factory.StartNew(async () => await repository.FindByTextSearch("bosch"))
                .GetAwaiter().GetResult().Result;

            foreach (var result in objects)
                Console.WriteLine(result.ToJson());

            //Call view
            var viewDocs = Task.Factory.StartNew(async () => await repository.GetDocumentFromDeveloperView())
                .GetAwaiter().GetResult().Result;

            foreach (var viewDoc in viewDocs)
                Console.WriteLine(viewDoc.ToJson());

            //Delete by id
            //var ok = Task.Factory.StartNew(async () => await repository.DeleteByIdAsync(savedObj.ID))
            //  .GetAwaiter().GetResult().Result;

            //Delete all objects
            //ok = Task.Factory.StartNew(async () => await repository.DeleteAllAsync())
            //  .GetAwaiter().GetResult().Result;

        }

        /// <summary>
        /// Make some query requests on the Dummy collection
        /// </summary>
        public static void DummyCollectionTest()
        {
            DummyRepository repository = new DummyRepository();

            //create indexes

            var ok = Task.Factory.StartNew(async () => await repository.BuildIndexKeys())
               .GetAwaiter().GetResult().Result;


            //Insert a new document
            var doc = Task.Factory.StartNew(async () => await repository.InsertDocumentAsync(repository.CreateDocument()))
                .GetAwaiter().GetResult().Result;

            //Update a document
            Task.Factory.StartNew(async () => await repository.UpdateDocumentAsync(doc)).GetAwaiter().GetResult();

            //Find all documents
            Task.Factory.StartNew(async () => await repository.FindAllDocumentsAsync()).GetAwaiter().GetResult();

            //Find a document
            var document =
            Task.Factory.StartNew(async () => await repository.FindDocumentByIdAsync(doc["_id"].AsObjectId))
             .GetAwaiter().GetResult().Result;

            if (document != null)
                Console.WriteLine(document.ToJson());

            //Find a document with balance greater than <value>
            var items =
            Task.Factory.StartNew(async () => await repository.FindDocumentByBalanaceGreaterThanAsync(5000))
             .GetAwaiter().GetResult().Result;
            foreach (var item in items)
                Console.WriteLine(item.ToJson());

            //Aggregation operations
            var results =
            Task.Factory.StartNew(async () => await repository.GroupDocumentByBalance())
            .GetAwaiter().GetResult().Result;

            foreach (var result in results)
                Console.WriteLine(result.ToJson());

            //Find with text serach pattern

            results = Task.Factory.StartNew(async () => await repository.FindDocumentsByTextSearch("euro"))
                .GetAwaiter().GetResult().Result;

            foreach (var result in results)
                Console.WriteLine(result.ToJson());

            // //Delete a document
            // Task.Factory.StartNew(async () => await repository.DeleteDocumentAsync(doc["_id"].AsObjectId))
            // .GetAwaiter().GetResult();

            // //Delete all documents
            // Task.Factory.StartNew(async () => await repository.DeleteAllDocumentAsync())
            //.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Make some requests on the Department collection
        /// </summary>
        public static void DepartmentCollectionTest()
        {
            //Save a new document
            DepartmentRepository repository = new DepartmentRepository();
            var obj = Factory.CreateDepartment();

            Task.Run(async () =>   await repository.SaveAsync(obj))
                .GetAwaiter().GetResult();

            //Find all documents
            var docs = Task.Factory.StartNew(async () => await repository.FindAllAsync())
               .GetAwaiter().GetResult().Result;

            foreach (var doc in docs)
                Console.WriteLine(doc.ToJson());
        }


        public static void CreateDummyDB()
        {
            Task.Factory.StartNew(async () => await MongoClientManager.CreateDummyDataBase())
                        .GetAwaiter().GetResult();
        }

        public static void DropDummyDB()
        {
            Task.Factory.StartNew(async () => await MongoClientManager.DropDummyDataBase())
                        .GetAwaiter().GetResult();
        }

    }
}
