using Client.Core.Model;
using MongoDB.Bson;
using System.Collections.ObjectModel;

namespace WebClient
{
    public class DeveloperItem : IDeveloper
    {
        public ObjectId ID { get; set; }

        public string Name { get; set; }

        public string CompanyName { get; set; }

        public string Gender { get; set; }

        public ObservableCollection<KnowledgeItem> Knowledges { get; set; }


        public bool IsValid { get; }
    }
}
