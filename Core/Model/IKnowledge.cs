namespace Client.Core.Model
{
    public interface IKnowledge
    {
        string Language { get; set; }
        string Technology { get; set; }
        ushort Rating { get; set; }
    }
}
