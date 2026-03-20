namespace ECommerce.Interfaces;

public interface IPaymentGateway
{
    bool Charge(decimal amount, string token);
}