using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace asp.Data
{
    public class BatDongSan
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string TieuDe { get; set; } = null!;
        public string DiaChi { get; set; } = null!;
        public string? MoTa { get; set; }
        public double Gia { get; set; }
        public string LoaiHinh { get; set; } = null!;
        public double DienTich { get; set; }
        public int PhongNgu { get; set; }
        public string? HinhAnhUrl { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? ProjectId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? OwnerId { get; set; }
    }
}