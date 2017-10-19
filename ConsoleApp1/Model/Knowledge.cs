using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbConsole.Model
{
    public class Knowledge
    {
        [BsonElement("language_name")]
        public string Language { get; set; }
        [BsonElement("technology")]
        public string Technology { get; set; }
        [BsonElement("rating")]
        public ushort Rating { get; set; }
    }
}
