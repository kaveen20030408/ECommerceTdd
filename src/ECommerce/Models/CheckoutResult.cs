namespace ECommerce.Models;

public class CheckoutResult
{
    public bool Success { get; }
    public string Message { get; }

    public CheckoutResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}