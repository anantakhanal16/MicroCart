using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OrderApi.Model
{
    public class OrderDetail
    {
        [BsonElement("product_id")]
        public string ProductId { get; set; }

        [BsonElement("quantity"), BsonRepresentation(BsonType.Decimal128)]
        public int Quantity { get; set; }

        [BsonElement("price"), BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }

        [BsonElement("unit_price"), BsonRepresentation(BsonType.Decimal128)]
        public decimal UnitPrice { get; set; }
    }
}
