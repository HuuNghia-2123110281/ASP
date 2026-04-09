using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace asp.Data
{
    public class Appointment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string CustomerId { get; set; } = null!;

        [BsonRepresentation(BsonType.ObjectId)]
        public string PropertyId { get; set; } = null!;

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = null!;

        [BsonElement("AppointmentDate")]
        public DateTime AppointmentDate { get; set; }

        [BsonElement("Status")]
        public string Status { get; set; } = "Đã lên lịch";
    }
}