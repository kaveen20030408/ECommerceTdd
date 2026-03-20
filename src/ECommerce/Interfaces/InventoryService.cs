namespace ECommerce.Interfaces;

public interface InventoryService
{
    int GetAvailable(string sku);
}