using ECommerce.Interfaces;
using ECommerce.Models;
using ECommerce.Services;
using Xunit;

namespace ECommerce.Tests;

public class OrderTests
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

        public bool Charge(decimal amount, string token)
        {
            return ShouldSucceed;
        }
    }

    private class FakeOrderRepository : IOrderRepository
    {
        public List<Order> SavedOrders { get; } = new();

        public void Save(Order order)
        {
            SavedOrders.Add(order);
        }
    }

    [Fact]
    public void Checkout_Should_Create_Order_When_Payment_Succeeds()
    {
        var catalog = new Catalog();
        catalog.AddProduct(new Product("P001", "Mouse", 500));

        var inventory = new FakeInventoryService();
        inventory.SetStock("P001", 5);

        var cart = new Cart(catalog, inventory);
        cart.AddItem("P001", 2);

        var paymentGateway = new FakePaymentGateway { ShouldSucceed = true };
        var orderRepository = new FakeOrderRepository();

        var discountEngine = new DiscountEngine(new List<IDiscountRule>
        {
            new BulkDiscountRule(),
            new OrderDiscountRule()
        });

        var checkoutService = new CheckoutService(
            inventory,
            paymentGateway,
            discountEngine,
            orderRepository);

        var result = checkoutService.Checkout(cart, "tok_test_123");

        Assert.True(result.Success);
        Assert.Single(orderRepository.SavedOrders);

        var order = orderRepository.SavedOrders[0];
        Assert.Equal(950, order.Total);
        Assert.Single(order.Items);
        Assert.Equal("P001", order.Items[0].Product.Sku);
    }

    [Fact]
    public void Checkout_Should_Not_Create_Order_When_Payment_Fails()
    {
        var catalog = new Catalog();
        catalog.AddProduct(new Product("P001", "Mouse", 500));

        var inventory = new FakeInventoryService();
        inventory.SetStock("P001", 5);

        var cart = new Cart(catalog, inventory);
        cart.AddItem("P001", 2);

        var paymentGateway = new FakePaymentGateway { ShouldSucceed = false };
        var orderRepository = new FakeOrderRepository();

        var discountEngine = new DiscountEngine(new List<IDiscountRule>
        {
            new BulkDiscountRule(),
            new OrderDiscountRule()
        });

        var checkoutService = new CheckoutService(
            inventory,
            paymentGateway,
            discountEngine,
            orderRepository);

        var result = checkoutService.Checkout(cart, "tok_test_123");

        Assert.False(result.Success);
        Assert.Empty(orderRepository.SavedOrders);
    }
}