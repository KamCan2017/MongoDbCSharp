using Core;
using Developer;
using Prism.Events;

namespace Client.Developer
{
    public class EntityEditPubEvent: PubSubEvent<DeveloperModel>
    {
    }

    public class UpdateDeveloperListPubEvent : PubSubEvent
    {
    }

    public class AddKnowledgePubEvent : PubSubEvent<KnowledgeModel>
    {
    }

}
