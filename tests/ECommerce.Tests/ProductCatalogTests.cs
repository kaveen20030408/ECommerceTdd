using ECommerce.Models;
using ECommerce.Services;
using Xunit;

namespace ECommerce.Tests;

public class ProductCatalogTests
{
    [Fact]
    public void Create_Product_With_Valid_Data()
    {
        var product = new Product("P001", "Mouse", 2500);

        Assert.Equal("P001", product.Sku);
        Assert.Equal("Mouse", product.Name);
        Assert.Equal(2500, product.Price);
    }

    [Fact]
    public void Create_Product_With_Negative_Price_Should_Throw_Exception()
    {
        Assert.Throws<ArgumentException>(() =>
            new Product("P001", "Mouse", -100));
    }

    [Fact]
    public void Create_Product_Without_Sku_Should_Throw_Exception()
    {
        Assert.Throws<ArgumentException>(() =>
            new Product("", "Mouse", 100));
    }

    [Fact]
    public void Create_Product_Without_Name_Should_Throw_Exception()
    {
        Assert.Throws<ArgumentException>(() =>
            new Product("P001", "", 100));
    }

    [Fact]
    public void Catalog_Should_Return_Product_By_Sku()
    {
        var catalog = new Catalog();
        var product = new Product("P001", "Mouse", 2500);

        catalog.AddProduct(product);

        var result = catalog.GetBySku("P001");

        Assert.NotNull(result);
        Assert.Equal("P001", result!.Sku);
        Assert.Equal("Mouse", result.Name);
    }

    [Fact]
    public void Catalog_Should_Return_Null_For_Missing_Sku()
    {
        var catalog = new Catalog();

        var result = catalog.GetBySku("P999");

        Assert.Null(result);
    }
}