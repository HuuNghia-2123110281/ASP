using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace asp.Data
{
    public class Transaction
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("PropertyId")]
        public string PropertyId { get; set; } = null!;

        [BsonElement("CustomerId")]
        public string CustomerId { get; set; } = null!;

        [BsonElement("UserId")]
        public string? UserId { get; set; }

        [BsonElement("Type")]
        public string Type { get; set; } = "Mua bán";

        [BsonElement("Price")]
        public double Price { get; set; }

        [BsonElement("Status")]
        public string Status { get; set; } = "Đang xử lý";

        [BsonElement("Date")]
        public DateTime Date { get; set; } = DateTime.Now;
    }
}