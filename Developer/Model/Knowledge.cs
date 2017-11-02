﻿using Client.Core.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace Developer
{
    public class KnowledgeModel : IKnowledge
    {
        [BsonElement("language_name")]
        public string Language { get; set; }
        [BsonElement("technology")]
        public string Technology { get; set; }
        [BsonElement("rating")]
        public ushort Rating { get; set; }
    }
}