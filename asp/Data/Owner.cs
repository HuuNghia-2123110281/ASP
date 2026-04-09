using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace asp.Data
{
    public class Owner
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("FullName")]
        public string FullName { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string IDCard { get; set; } = null!;

        public string? Address { get; set; }
    }
}