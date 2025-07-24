using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OrderApi.Model
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string OrderId { get; set; }

        [BsonElement("customer_id")]
        public int CustomerId { get; set; }

        [BsonElement("ordered_on")]
        public string OrderedOn { get; set; }

        [BsonElement("order_details")]
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
