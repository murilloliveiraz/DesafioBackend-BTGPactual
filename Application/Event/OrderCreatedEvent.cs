using Core.Entities;

namespace Application.Event
{
    public class OrderCreatedEvent
    {
        public long codigoPedido { get; set; }
        public long codigoCliente { get; set; }
        public List<OrderItemEvent> itens { get; set; } = new List<OrderItemEvent>();

        public Order ToEntity()
            => new Order(
                orderId: codigoPedido,
                customerId: codigoCliente,
                total: itens.Sum(i => i.preco * i.quantidade),
                itens: itens.Select(i => i.ToEntity()).ToList()
            );
    }
}
