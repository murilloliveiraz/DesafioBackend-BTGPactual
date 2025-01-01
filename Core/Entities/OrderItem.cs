namespace Core.Entities
{
    public class OrderItem
    {
        public OrderItem(string product, int quantity, decimal price)
        {
            this.product = product;
            this.quantity = quantity;
            this.price = price;
        }

        public string product { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
    }
}
