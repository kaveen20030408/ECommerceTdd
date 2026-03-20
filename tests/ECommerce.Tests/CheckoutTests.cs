using ECommerce.Interfaces;
using ECommerce.Models;
using ECommerce.Services;
using Xunit;

namespace ECommerce.Tests;

public class CheckoutTests
{
    private class FakeInventoryService : IInventoryService
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

    private class FakePaymentGateway : IPaymentGateway
    {
        public bool ShouldSucceed { get; set; }
        public decimal LastChargedAmount { get; private set; }
        public string? LastToken { get; private set; }

        public bool Charge(decimal amount, string token)
        {
            LastChargedAmount = amount;
            LastToken = token;
            return ShouldSucceed;
        }
    }

    [Fact]
    public void Checkout_Should_Succeed_When_Payment_Is_Successful()
    {
        var catalog = new Catalog();
        catalog.AddProduct(new Product("P001", "Mouse", 500));

        var inventory = new FakeInventoryService();
        inventory.SetStock("P001", 5);

        var cart = new Cart(catalog, inventory);
        cart.AddItem("P001", 2);

        var paymentGateway = new FakePaymentGateway { ShouldSucceed = true };

        var discountEngine = new DiscountEngine(new List<IDiscountRule>
        {
            new BulkDiscountRule(),
            new OrderDiscountRule()
        });

        var checkoutService = new CheckoutService(inventory, paymentGateway, discountEngine);

        var result = checkoutService.Checkout(cart, "tok_test_123");

        Assert.True(result.Success);
        Assert.Equal("Checkout successful.", result.Message);
        Assert.Equal(950, paymentGateway.LastChargedAmount);
        Assert.Equal("tok_test_123", paymentGateway.LastToken);
    }

    [Fact]
    public void Checkout_Should_Fail_When_Payment_Fails()
    {
        var catalog = new Catalog();
        catalog.AddProduct(new Product("P001", "Mouse", 500));

        var inventory = new FakeInventoryService();
        inventory.SetStock("P001", 5);

        var cart = new Cart(catalog, inventory);
        cart.AddItem("P001", 2);

        var paymentGateway = new FakePaymentGateway { ShouldSucceed = false };

        var discountEngine = new DiscountEngine(new List<IDiscountRule>
        {
            new BulkDiscountRule(),
            new OrderDiscountRule()
        });

        var checkoutService = new CheckoutService(inventory, paymentGateway, discountEngine);

        var result = checkoutService.Checkout(cart, "tok_test_123");

        Assert.False(result.Success);
        Assert.Equal("Payment failed.", result.Message);
    }

    [Fact]
    public void Checkout_Should_Fail_When_Inventory_Is_No_Longer_Available()
    {
        var catalog = new Catalog();
        catalog.AddProduct(new Product("P001", "Mouse", 500));

        var inventory = new FakeInventoryService();
        inventory.SetStock("P001", 1);

        var cart = new Cart(catalog, inventory);
        cart.AddItem("P001", 1);

        inventory.SetStock("P001", 0);

        var paymentGateway = new FakePaymentGateway { ShouldSucceed = true };

        var discountEngine = new DiscountEngine(new List<IDiscountRule>
        {
            new BulkDiscountRule(),
            new OrderDiscountRule()
        });

        var checkoutService = new CheckoutService(inventory, paymentGateway, discountEngine);

        var result = checkoutService.Checkout(cart, "tok_test_123");

        Assert.False(result.Success);
        Assert.Equal("Item P001 is no longer available in requested quantity.", result.Message);
    }
}