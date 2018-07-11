using Client.Core.Model;
using Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Core
{
    [DataContract]
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
            KnowledgeIds = new List<string>();
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [DataMember]
        public string ID { get; set; }

        [BsonElement("name")]
        [DataMember]
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
        [DataMember]
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
        [DataMember]
        public string Gender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                NotifyPropertyChanged(nameof(Gender));
            }
        }

        [DataMember]
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
        [DataMember]
        public List<string> KnowledgeIds { get; set; }

        public bool IsValid
        {
            get { return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(CompanyName); }
        }
    }

    
}
