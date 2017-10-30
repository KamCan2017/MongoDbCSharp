using Client.Core.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Client.Developer
{
    public class Developer : IDeveloper
    {
        [BsonConstructor]
        public Developer()
        {
            KnowledgeBase = new List<IKnowledge>();
        }

        [BsonId]
        public ObjectId ID { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("company_name")]
        public string CompanyName { get; set; }

        [BsonElement("knowledge_base")]
        public List<IKnowledge> KnowledgeBase { get; set; }
    }

    
}
