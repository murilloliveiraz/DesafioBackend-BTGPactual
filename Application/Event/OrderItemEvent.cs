using Core.Entities;

namespace Application.Event
{
    public class OrderItemEvent
    {
        public string produto { get; set; }
        public int quantidade { get; set; }
        public decimal preco { get; set; }

        public OrderItem ToEntity()
           => new OrderItem(produto, quantidade, preco);
    }
}
