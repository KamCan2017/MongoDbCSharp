using MongoDB.Bson;

namespace MongoDb.WebApi.Models
{
    public class Knowledge
    {
        public ObjectId ID { get; set; }

        public string Language { get; set; }

        public string Technology { get; set; }

        public ushort Rating { get; set; }

        public bool IsValid { get; set; }
    }

}