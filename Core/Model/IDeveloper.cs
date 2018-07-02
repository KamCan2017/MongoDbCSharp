using MongoDB.Bson;

namespace Client.Core.Model
{
    public interface IDeveloper
    {
        string ID { get; set; }

        string Name { get; set; }

        string CompanyName { get; set; }

        bool IsValid { get; }

        string Gender { get; set; }
    }    
}
