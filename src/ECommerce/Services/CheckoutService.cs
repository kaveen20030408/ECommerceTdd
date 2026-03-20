using ECommerce.Interfaces;
using ECommerce.Models;

namespace ECommerce.Services;

public class CheckoutService
{
    private readonly IInventoryService _inventoryService;
    private readonly IPaymentGateway _paymentGateway;
    private readonly DiscountEngine _discountEngine;
    private readonly IOrderRepository? _orderRepository;

    public CheckoutService(
        IInventoryService inventoryService,
        IPaymentGateway paymentGateway,
        DiscountEngine discountEngine,
        IOrderRepository? orderRepository = null)
    {
        _inventoryService = inventoryService;
        _paymentGateway = paymentGateway;
        _discountEngine = discountEngine;
        _orderRepository = orderRepository;
    }

    public CheckoutResult Checkout(Cart cart, string token)
    {
        foreach (var item in cart.Items)
        {
            var available = _inventoryService.GetAvailable(item.Product.Sku);
            if (item.Quantity > available)
            {
                return new CheckoutResult(
                    false,
                    $"Item {item.Product.Sku} is no longer available in requested quantity.");
            }
        }

        var finalTotal = _discountEngine.CalculateFinalTotal(cart);

        var paymentSuccess = _paymentGateway.Charge(finalTotal, token);
        if (!paymentSuccess)
        {
            return new CheckoutResult(false, "Payment failed.");
        }

        if (_orderRepository != null)
        {
            var order = new Order(cart.Items.ToList(), finalTotal, DateTime.UtcNow);
            _orderRepository.Save(order);
        }

        return new CheckoutResult(true, "Checkout successful.");
    }
}