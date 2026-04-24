using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace asp.Data
{
    public class PropertyImage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string PropertyId { get; set; } = null!;

        [BsonElement("ImageUrl")]
        public string ImageUrl { get; set; } = null!;

        public bool IsMain { get; set; }
    }
}