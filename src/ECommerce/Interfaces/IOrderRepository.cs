using ECommerce.Models;

namespace ECommerce.Interfaces;

public interface IOrderRepository
{
    void Save(Order order);
}