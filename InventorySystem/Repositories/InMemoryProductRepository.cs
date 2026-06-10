using InventorySystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InventorySystem.Repositories;

public class InMemoryProductRepository : IProductRepository
{
    private readonly List<Product> _products = new();

    public void Add(Product product)
    {
        _products.Add(product);
    }

    public Product? GetByName(string name)
    {
        return _products.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}