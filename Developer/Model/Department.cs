using Client.Core.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace Client.Developer
{

    [BsonIgnoreExtraElements]
    public class Department : IDepartment
    {
        public string Name { get; set; }

       public string Responsible { get; set; }
    }
}
