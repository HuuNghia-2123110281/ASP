using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace asp.Data
{
    public class Contract
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        // Liên kết với _id của bảng Transactions
        [BsonRepresentation(BsonType.ObjectId)]
        public string TransactionId { get; set; } = null!;

        [BsonElement("ContractNumber")]
        public string ContractNumber { get; set; } = null!;

        [BsonElement("SignDate")]
        public DateTime SignDate { get; set; }

        [BsonElement("FileUrl")]
        public string? FileUrl { get; set; }
    }
}