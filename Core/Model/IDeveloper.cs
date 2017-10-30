using MongoDB.Bson;
using System.Collections.Generic;

namespace Client.Core.Model
{
    public interface IDeveloper
    {
        
        ObjectId ID { get; set; }

        string Name { get; set; }

        string CompanyName { get; set; }
    }

    
}
