using Client.Core.Model;
using Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.Serialization;

namespace Core
{
    [DataContract]
    public class KnowledgeModel : BasePropertyChanged, IKnowledge
    {
        private string _language;
        private string _technology;

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [DataMember]
        public string ID { get; set; }

        [BsonElement("language_name")]
        [DataMember]
        public string Language
        {
            get { return _language; }
            set
            {
                _language = value;
                NotifyPropertyChanged(nameof(Language));
            }
        }

        [BsonElement("technology")]
        [DataMember]
        public string Technology
        {
            get { return _technology; }
            set
            {
                _technology = value;
                NotifyPropertyChanged(nameof(Technology));
            }
        }

        [BsonElement("rating")]
        [DataMember]
        public ushort Rating { get; set; }

        public bool IsValid
        {
            get { return !string.IsNullOrEmpty(Language) && !string.IsNullOrEmpty(Technology); }
        }
    }
}
