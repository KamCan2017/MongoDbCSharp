using Client.Core.Model;

namespace Client.Developer
{
    public static class Factory
    {
        public static IDeveloper CreateDeveloper()
        {
            var developer = new Developer
            {
                Name = "asty",
                CompanyName = "cellent"
            };

            Knowledge knowledge = new Knowledge
            {
                Language = "C#",
                Technology = "WPF",
                Rating = 1
            };

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
