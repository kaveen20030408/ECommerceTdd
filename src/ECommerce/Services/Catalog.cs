using ECommerce.Models;

namespace ECommerce.Services;

public class Catalog
{
    private readonly Dictionary<string, Product> _products = new();

    public void AddProduct(Product product)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));

        _products[product.Sku] = product;
    }

    public Product? GetBySku(string sku)
    {
        if (string.IsNullOrWhiteSpace(sku))
            return null;

        _products.TryGetValue(sku, out var product);
        return product;
    }
}