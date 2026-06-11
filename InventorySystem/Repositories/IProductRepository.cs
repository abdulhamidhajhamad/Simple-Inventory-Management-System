using InventorySystem.Domain;
using System.Collections.Generic;

namespace InventorySystem.Repositories;

public interface IProductRepository
{
    void Add(Product product);
    Product? GetByName(string name);
    IReadOnlyCollection<Product> GetAll();
    void Update(string oldName, Product updatedProduct); 
    
}