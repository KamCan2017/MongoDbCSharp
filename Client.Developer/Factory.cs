using Client.Core.Model;

namespace Client.Developer
{
    public static class Factory
    {
        public static IDeveloper CreateDeveloper()
        {
            var developer = new Developer();
            developer.Name = "asty";
            developer.CompanyName = "cellent";

            IKnowledge knowledge = new Knowledge();
            knowledge.Language = "C#";
            knowledge.Technology = "WPF";
            knowledge.Rating = 1;

            developer.KnowledgeBase.Add(knowledge);
            return developer;
        }

        public static IDepartment CreateDepartment()
        {
            IDepartment obj = new Department() { Name = "HR", Responsible = "Camara, Jason"};

            return obj;
        }
    }
}
