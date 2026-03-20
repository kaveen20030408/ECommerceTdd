using ECommerce.Services;

namespace ECommerce.Interfaces;

public interface IDiscountRule
{
    decimal Apply(Cart cart, decimal currentTotal);
}