using ECommerce.Models;
using ECommerce.Services;
using ECommerce.Interfaces;
using Xunit;

namespace ECommerce.Tests;

public class InventoryCartTests
{
    private class FakeInventoryService : InventoryService
    {
        private readonly Dictionary<string, int> _stock = new();

        public void SetStock(string sku, int quantity)
        {
            _stock[sku] = quantity;
        }

        public int GetAvailable(string sku)
        {
            return _stock.TryGetValue(sku, out var quantity) ? quantity : 0;
        }
    }

    [Fact]
    public void Add_Item_Should_Fail_When_Requested_Quantity_Exceeds_Available_Stock()
    {
        var catalog = new Catalog();
        catalog.AddProduct(new Product("P001", "Mouse", 2500));

        var inventory = new FakeInventoryService();
        inventory.SetStock("P001", 2);

        var cart = new Cart(catalog, inventory);

        var ex = Assert.Throws<InvalidOperationException>(() => cart.AddItem("P001", 3));
        Assert.Equal("Insufficient inventory available.", ex.Message);
    }

    [Fact]
    public void Add_Item_Should_Succeed_When_Requested_Quantity_Is_Within_Available_Stock()
    {
        var catalog = new Catalog();
        catalog.AddProduct(new Product("P001", "Mouse", 2500));

        var inventory = new FakeInventoryService();
        inventory.SetStock("P001", 5);

        var cart = new Cart(catalog, inventory);

        cart.AddItem("P001", 2);

        Assert.Single(cart.Items);
        Assert.Equal(2, cart.Items[0].Quantity);
    }
}