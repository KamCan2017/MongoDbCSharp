using Client.Core.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Developer
{
    public class DeveloperModel : IDeveloper
    {
        [BsonConstructor]
        public DeveloperModel()
        {
            KnowledgeBase = new List<KnowledgeModel>();
        }

        [BsonId]
        public ObjectId ID { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("company_name")]
        public string CompanyName { get; set; }

        [BsonElement("knowledge_base")]
        public List<KnowledgeModel> KnowledgeBase { get; set; }

        public bool IsValid
        {
            get { return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(CompanyName); }
        }
    }

    
}
