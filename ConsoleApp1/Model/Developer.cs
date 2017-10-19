using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MongoDbConsole.Model
{
    public class Developer
    {
        [BsonConstructor]
        public Developer()
        {
            KnowledgeBase = new List<Knowledge>();
        }

        [BsonId]
        public ObjectId ID { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("company_name")]
        public string CompanyName { get; set; }

        [BsonElement("knowledge_base")]
        public List<Knowledge> KnowledgeBase { get; set; }
    }

    
}
