using Core.Entities;

namespace API.DTOs
{
    public record OrderResponse
    {
        public long orderId { get; set; }
        public long customerId { get; set; }
        public decimal total { get; set; }

        public OrderResponse ToOrderResponse(Order order)
        {
            return new OrderResponse
            {
                orderId = order.orderId,
                customerId = order.customerId,
                total = order.itens.Sum(i => i.price * i.quantity)
            };
        }
    }
}
