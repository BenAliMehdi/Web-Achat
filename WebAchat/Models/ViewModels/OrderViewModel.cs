namespace WebAchat.Models.ViewModels
{
    public class OrderViewModel
    {
        public int TempOrderId { get; set; }
        public DateTime Date { get; set; }
        public List<OrderProductViewModel> Items { get; set; } = new List<OrderProductViewModel>();
        public decimal TotalAmount { get; set; }
    }


}