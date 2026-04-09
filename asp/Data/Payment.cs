using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace asp.Data
{
    public class Payment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        // Liên kết với _id của bảng Transactions
        [BsonRepresentation(BsonType.ObjectId)]
        public string TransactionId { get; set; } = null!;

        [BsonElement("Amount")]
        public double Amount { get; set; }

        [BsonElement("PaymentDate")]
        public DateTime PaymentDate { get; set; }

        [BsonElement("Method")]
        public string Method { get; set; } = "Chuyển khoản";
    }
}