namespace ECommerce.Models;

public class Product
{
    public string Sku { get; }
    public string Name { get; }
    public decimal Price { get; }

    public Product(string sku, string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(sku))
            throw new ArgumentException("SKU is required.");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.");

        if (price < 0)
            throw new ArgumentException("Price cannot be negative.");

        Sku = sku;
        Name = name;
        Price = price;
    }
}