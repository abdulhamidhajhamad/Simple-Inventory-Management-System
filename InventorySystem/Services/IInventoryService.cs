using System.Collections.Generic;
using InventorySystem.Common;
using InventorySystem.Domain;

namespace InventorySystem.Services;

public interface IInventoryService
{
    Result AddProduct(string name, decimal price, int quantity);
    bool IsNameDuplicate(string name);
    Result<IReadOnlyCollection<Product>> GetAllProducts();
}