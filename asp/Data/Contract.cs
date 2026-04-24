using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using Microsoft.AspNetCore.Http;

namespace asp.Data
{
    public class Contract
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string TransactionId { get; set; } = null!;

        [BsonElement("ContractNumber")]
        public string ContractNumber { get; set; } = null!;

        [BsonElement("SignDate")]
        public DateTime SignDate { get; set; } = DateTime.Now;

        [BsonElement("FileUrl")]
        public string? FileUrl { get; set; }
    }
    public class ContractRequest
    {
        public string TransactionId { get; set; } = "";
        public string ContractNumber { get; set; } = "";
        public IFormFile? File { get; set; }
    }
}