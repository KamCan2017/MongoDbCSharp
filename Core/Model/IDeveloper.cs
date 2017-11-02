﻿using MongoDB.Bson;

namespace Client.Core.Model
{
    public interface IDeveloper
    {
        
        ObjectId ID { get; set; }

        string Name { get; set; }

        string CompanyName { get; set; }

        bool IsValid { get; }
    }

    
}