using ECommerce.Interfaces;

namespace ECommerce.Services;

public class DiscountEngine
{
    private readonly IEnumerable<IDiscountRule> _rules;

    public DiscountEngine(IEnumerable<IDiscountRule> rules)
    {
        _rules = rules;
    }

    public decimal CalculateFinalTotal(Cart cart)
    {
        decimal total = cart.GetTotal();

        foreach (var rule in _rules)
        {
            total = rule.Apply(cart, total);
        }

        return total;
    }
}