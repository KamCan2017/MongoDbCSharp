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
}
