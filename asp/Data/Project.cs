using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace asp.Data
{
    public class Project
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Name { get; set; } = null!;
        public string? Investor { get; set; }
        public string? Location { get; set; }
        public string? Scale { get; set; }
        public string Status { get; set; } = "Đang triển khai";
    }
}