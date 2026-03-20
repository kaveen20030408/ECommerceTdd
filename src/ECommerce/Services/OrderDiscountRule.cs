using ECommerce.Interfaces;

namespace ECommerce.Services;

public class OrderDiscountRule : IDiscountRule
{
    public decimal Apply(Cart cart, decimal currentTotal)
    {
        if (cart.GetTotal() >= 1000m)
        {
            return currentTotal * 0.95m;
        }

        return currentTotal;
    }
}