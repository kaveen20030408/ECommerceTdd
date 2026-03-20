using ECommerce.Interfaces;
using ECommerce.Models;
using ECommerce.Services;
using Xunit;

namespace ECommerce.Tests;

public class DiscountTests
{
    [Fact]
    public void Bulk_Discount_Should_Apply_10_Percent_When_Quantity_Is_10_Or_More()
    {
        var catalog = new Catalog();
        catalog.AddProduct(new Product("P001", "Pen", 100));

        var cart = new Cart(catalog);
        cart.AddItem("P001", 10); // 100 * 10 = 1000

        var engine = new DiscountEngine(new List<IDiscountRule>
        {
            new BulkDiscountRule(),
            new OrderDiscountRule()
        });

        var finalTotal = engine.CalculateFinalTotal(cart);

        Assert.Equal(855, finalTotal);
    }

    [Fact]
    public void Order_Discount_Should_Apply_5_Percent_When_Subtotal_Is_1000_Or_More()
    {
        var catalog = new Catalog();
        catalog.AddProduct(new Product("P001", "Book", 250));
        catalog.AddProduct(new Product("P002", "Bag", 500));

        var cart = new Cart(catalog);
        cart.AddItem("P001", 2); // 500
        cart.AddItem("P002", 1); // 500
        // subtotal = 1000

        var engine = new DiscountEngine(new List<IDiscountRule>
        {
            new BulkDiscountRule(),
            new OrderDiscountRule()
        });

        var finalTotal = engine.CalculateFinalTotal(cart);

        Assert.Equal(950, finalTotal);
    }

    [Fact]
    public void No_Discount_Should_Be_Applied_When_Rules_Do_Not_Match()
    {
        var catalog = new Catalog();
        catalog.AddProduct(new Product("P001", "Notebook", 100));

        var cart = new Cart(catalog);
        cart.AddItem("P001", 2); // 200

        var engine = new DiscountEngine(new List<IDiscountRule>
        {
            new BulkDiscountRule(),
            new OrderDiscountRule()
        });

        var finalTotal = engine.CalculateFinalTotal(cart);

        Assert.Equal(200, finalTotal);
    }
}