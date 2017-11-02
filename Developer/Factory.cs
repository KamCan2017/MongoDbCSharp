﻿using Client.Core.Model;

namespace Developer
{
    public static class Factory
    {
        public static IDeveloper CreateDeveloper()
        {
            var developer = new DeveloperModel
            {
                Name = "asty",
                CompanyName = "cellent"
            };

            KnowledgeModel knowledge = new KnowledgeModel
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
