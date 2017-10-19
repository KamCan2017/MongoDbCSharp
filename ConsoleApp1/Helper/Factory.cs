using MongoDbConsole.Model;

namespace MongoDbConsole.Helper
{
    public static class Factory
    {
        public static Developer CreateDeveloper()
        {
            var developer = new Developer();
            developer.Name = "asty";
            developer.CompanyName = "cellent";

            Knowledge knowledge = new Knowledge();
            knowledge.Language = "C#";
            knowledge.Technology = "WPF";
            knowledge.Rating = 1;

            developer.KnowledgeBase.Add(knowledge);
            return developer;
        }

        public static Department CreateDepartment()
        {
            Department obj = new Department() { Name = "HR", Responsible = "Camara, Jason"};

            return obj;
        }
    }
}
