namespace ECommerce.Interfaces;

public interface IInventoryService
{
    int GetAvailable(string sku);
}