using ECommerce.Models;

namespace ECommerce.Services;

public class Cart
{
    private readonly Catalog _catalog;
    private readonly List<CartItem> _items = new();

    public IReadOnlyList<CartItem> Items => _items;

    public Cart(Catalog catalog)
    {
        _catalog = catalog;
    }

    public void AddItem(string sku, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        var product = _catalog.GetBySku(sku);
        if (product == null)
            throw new InvalidOperationException("Product not found in catalog.");

        var existingItem = _items.FirstOrDefault(i => i.Product.Sku == sku);

        if (existingItem != null)
        {
            existingItem.IncreaseQuantity(quantity);
        }
        else
        {
            _items.Add(new CartItem(product, quantity));
        }
    }

    public void RemoveItem(string sku)
    {
        var item = _items.FirstOrDefault(i => i.Product.Sku == sku);
        if (item != null)
        {
            _items.Remove(item);
        }
    }

    public decimal GetTotal()
    {
        return _items.Sum(i => i.Product.Price * i.Quantity);
    }
}