using MongoDB.Bson;
using System.Collections.ObjectModel;

namespace MongoDb.WebApi.Models
{
    public class Developer
    {       

        public ObjectId ID { get; set; }

        public string Name { get; set; }

        public string CompanyName { get; set; }


        public string Gender { get; set; }

        public bool IsValid { get; set; }


        public ObservableCollection<Knowledge> Knowledges { get; set; }
       
    }

}