using InventorySystem.Common;

namespace InventorySystem.Services;

public interface IInventoryService
{
    Result AddProduct(string name, decimal price, int quantity);
}