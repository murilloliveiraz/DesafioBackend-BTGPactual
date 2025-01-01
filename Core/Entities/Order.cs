using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Entities
{
    public class Order
    {
        public Order(long orderId, long customerId, decimal total, List<OrderItem> itens)
        {
            this.orderId = orderId;
            this.customerId = customerId;
            this.total = total;
            this.itens = itens ?? new List<OrderItem>();
        }

        [BsonId]
        public long orderId { get; set; }
        public long customerId { get; set; }
        public decimal total { get; set; }
        public List<OrderItem> itens { get; set; } = new List<OrderItem>();
    }
}
