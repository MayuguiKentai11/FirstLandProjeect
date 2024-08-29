namespace EntityLayer
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public Client IdClient { get; set; }
        public Product IdProduct { get; set; }
        public int orderProduct { get; set; }
        public int totalQuantityTypesProducts { get; set; }
    }
}
