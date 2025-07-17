namespace Core.Entites
{
    public class ShopingCart
    {
        public required string Id { get; set; }
        public List<CartItem?> Items { get; set; } =  new List<CartItem>();
    }
}
