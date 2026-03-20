using ECommerce.Interfaces;

namespace ECommerce.Services;

public class BulkDiscountRule : IDiscountRule
{
    public decimal Apply(Cart cart, decimal currentTotal)
    {
        decimal discount = 0m;

        foreach (var item in cart.Items)
        {
            if (item.Quantity >= 10)
            {
                discount += item.Product.Price * item.Quantity * 0.10m;
            }
        }

        return currentTotal - discount;
    }
}