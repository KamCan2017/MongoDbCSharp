using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebClient
{
    public class KnowledgeItem
    {
        public ObjectId ID { get; set; }

        public string Language { get; set; }

        public string Technology { get; set; }

        public ushort Rating { get; set; }

        public bool IsValid { get; set; }
    }
}
