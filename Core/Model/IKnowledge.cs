using MongoDB.Bson;

namespace Client.Core.Model
{
    public interface IKnowledge
    {
        ObjectId ID { get; set; }

        string Language { get; set; }

        string Technology { get; set; }

        ushort Rating { get; set; }

        bool IsValid { get; }
    }
}
