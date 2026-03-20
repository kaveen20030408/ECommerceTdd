namespace ECommerce.Models;

public class Order
{
    public List<CartItem> Items { get; }
    public decimal Total { get; }
    public DateTime Timestamp { get; }

    public Order(List<CartItem> items, decimal total, DateTime timestamp)
    {
        Items = items;
        Total = total;
        Timestamp = timestamp;
    }
}