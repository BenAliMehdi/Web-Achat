﻿namespace WebAchat.Models.ViewModels
{
    public class OrderProductViewModel
    {
        public int TempOrderProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total => Price * Quantity;
        public bool IsTemporary { get; set; }
    }
}
