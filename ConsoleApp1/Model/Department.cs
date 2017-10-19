using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbConsole.Model
{

    [BsonIgnoreExtraElements]
    public class Department
    {
        public string Name { get; set; }

       public string Responsible { get; set; }
    }
}
