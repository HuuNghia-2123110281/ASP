using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace asp.Data
{
    public class Customer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("FullName")]
        public string FullName { get; set; } = "";

        [BsonElement("Phone")]
        public string Phone { get; set; } = "";

        [BsonElement("Email")]
        public string? Email { get; set; } = "";

        [BsonElement("Address")]
        public string? Address { get; set; } = "";

        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}