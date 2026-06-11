using InventorySystem.Domain;
using System;
using System.Collections.Generic;

namespace InventorySystem.Repositories;

public class InMemoryProductRepository : IProductRepository
{
    private readonly List<Product> _products = new();
    
    private readonly Dictionary<string, Product> _productsByNameIndex = new(StringComparer.OrdinalIgnoreCase);

    public void Add(Product product)
    {
        _products.Add(product);
        _productsByNameIndex[product.Name] = product;
    }

    public Product? GetByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return null;

        return _productsByNameIndex.TryGetValue(name, out var product) ? product : null;
    }

    public IReadOnlyCollection<Product> GetAll()
    {
        return _products.AsReadOnly();
    }
}