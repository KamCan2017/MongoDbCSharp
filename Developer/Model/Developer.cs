using Client.Core.Model;
using Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Developer
{
    public class DeveloperModel : BasePropertyChanged, IDeveloper
    {
        private string _name;
        private string _companyName;
        private ObservableCollection<KnowledgeModel> _knowledgeCollection;
        private string _gender;

        [BsonConstructor]
        public DeveloperModel()
        {
            KnowledgeBase = new ObservableCollection<KnowledgeModel>();
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


        [BsonElement("gender")]
        public string Gender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                NotifyPropertyChanged(nameof(Gender));
            }
        }

        public ObservableCollection<KnowledgeModel> KnowledgeBase
        {
            get { return _knowledgeCollection; }
            set
            {
                _knowledgeCollection = value;
                NotifyPropertyChanged(nameof(KnowledgeBase));
            }
        }

        [BsonElement("knowledge_Ids")]
        public List<ObjectId> KnowledgeIds { get; set; }

        public bool IsValid
        {
            get { return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(CompanyName); }
        }
    }

    
}
