using Client.Core.Model;
using Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Developer
{
    public class KnowledgeModel : BasePropertyChanged, IKnowledge
    {
        private string _language;
        private string _technology;

        [BsonId]
        public ObjectId ID { get; set; }

        [BsonElement("language_name")]
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
        public ushort Rating { get; set; }

        public bool IsValid
        {
            get { return !string.IsNullOrEmpty(Language) && !string.IsNullOrEmpty(Technology); }
        }
    }
}
