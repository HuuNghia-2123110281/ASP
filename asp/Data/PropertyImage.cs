using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace asp.Data
{
    public class PropertyImage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        // Khóa ngoại liên kết với _id của Collection BatDongSans
        [BsonRepresentation(BsonType.ObjectId)]
        public string PropertyId { get; set; } = null!;

        [BsonElement("ImageUrl")]
        public string ImageUrl { get; set; } = null!;

        // Đánh dấu true nếu đây là ảnh bìa hiển thị ngoài trang chủ
        public bool IsMain { get; set; }
    }
}