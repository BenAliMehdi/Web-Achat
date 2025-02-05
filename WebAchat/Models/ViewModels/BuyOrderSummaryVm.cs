namespace WebAchat.Models.ViewModels
{
    public class BuyOrderSummaryVm
    {
        public int TempOrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<BuyOrderItemVm> Items { get; set; } = new List<BuyOrderItemVm>();
        public decimal TotalAmount { get; set; }
    }

    public class BuyOrderItemVm
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total => Price * Quantity;
    }
}