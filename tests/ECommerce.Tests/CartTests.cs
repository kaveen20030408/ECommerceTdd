using ECommerce.Models;
using ECommerce.Services;
using Xunit;

namespace ECommerce.Tests;

public class CartTests
{
    [Fact]
    public void Add_Item_To_Cart_Should_Add_Product()
    {
        var catalog = new Catalog();
        var product = new Product("P001", "Mouse", 2500);
        catalog.AddProduct(product);

        var cart = new Cart(catalog);

        cart.AddItem("P001", 2);

        Assert.Single(cart.Items);
        Assert.Equal("P001", cart.Items[0].Product.Sku);
        Assert.Equal(2, cart.Items[0].Quantity);
    }

    [Fact]
    public void Add_Item_With_Invalid_Quantity_Should_Throw_Exception()
    {
        var catalog = new Catalog();
        var product = new Product("P001", "Mouse", 2500);
        catalog.AddProduct(product);

        var cart = new Cart(catalog);

        Assert.Throws<ArgumentException>(() => cart.AddItem("P001", 0));
    }

    [Fact]
    public void Add_Item_Not_In_Catalog_Should_Throw_Exception()
    {
        var catalog = new Catalog();
        var cart = new Cart(catalog);

        Assert.Throws<InvalidOperationException>(() => cart.AddItem("P999", 1));
    }

    [Fact]
    public void Remove_Item_Should_Remove_Product_From_Cart()
    {
        var catalog = new Catalog();
        var product = new Product("P001", "Mouse", 2500);
        catalog.AddProduct(product);

        var cart = new Cart(catalog);
        cart.AddItem("P001", 2);

        cart.RemoveItem("P001");

        Assert.Empty(cart.Items);
    }

    [Fact]
    public void Get_Total_Should_Return_Correct_Total()
    {
        var catalog = new Catalog();
        catalog.AddProduct(new Product("P001", "Mouse", 2500));
        catalog.AddProduct(new Product("P002", "Keyboard", 5000));

        var cart = new Cart(catalog);
        cart.AddItem("P001", 2);
        cart.AddItem("P002", 1);

        var total = cart.GetTotal();

        Assert.Equal(10000, total);
    }
}