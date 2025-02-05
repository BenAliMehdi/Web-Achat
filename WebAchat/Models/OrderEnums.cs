namespace WebAchat.Models
{
    public enum OrderStatus
    {
        Pending,
        Processing,
        Delivered,
        Cancelled,
        Delayed
    }

    public enum OrderType
    {
        Purchase,
        Sales
    }
}
