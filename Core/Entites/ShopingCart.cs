﻿namespace Core.Entites
{
    public class ShopingCart
    {
        public required string Id { get; set; }
        public int DeliveryMethodId { get; set; }
        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
        public List<CartItem?> Items { get; set; } = [];
    }
}
