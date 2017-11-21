using Client.Core.Model;
using Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Developer
{
    public class DeveloperModel : BasePropertyChanged, IDeveloper
    {
        private string _name;
        private string _companyName;

        [BsonConstructor]
        public DeveloperModel()
        {
            KnowledgeBase = new List<KnowledgeModel>();
            KnowledgeIds = new List<ObjectId>();
        }

        [BsonId]
        public ObjectId ID { get; set; }

        [BsonElement("name")]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged(nameof(Name));
            }
        }

        [BsonElement("company_name")]
        public string CompanyName
        {
            get { return _companyName; }
            set
            {
                _companyName = value;
                NotifyPropertyChanged(nameof(CompanyName));
            }
        }

        public List<KnowledgeModel> KnowledgeBase { get; set; }

        [BsonElement("knowledge_Ids")]
        public List<ObjectId> KnowledgeIds { get; set; }

        public bool IsValid
        {
            get { return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(CompanyName); }
        }
    }

    
}
