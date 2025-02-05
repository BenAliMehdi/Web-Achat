namespace WebAchat.Models.ViewModels
{
    public class CurrentOrderVm
    {
        public int TempOrderId { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<OrderItemVm> Items { get; set; }
        public decimal TotalAmount { get; set; }
    }

}
