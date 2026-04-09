using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace asp.Data
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? Slug { get; set; }

        public string? Icon { get; set; }

        public int DisplayOrder { get; set; }
    }
}