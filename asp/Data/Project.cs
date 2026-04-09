using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace asp.Data
{
    public class Project
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; } = null!;

        public string Investor { get; set; } = null!;

        public string Location { get; set; } = null!;

        public string? Scale { get; set; }

        public string? Status { get; set; }
    }
}