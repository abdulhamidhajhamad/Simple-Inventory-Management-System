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
public void Update(string oldName, Product updatedProduct)
{
    if (string.IsNullOrWhiteSpace(oldName) || updatedProduct == null) return;

    var index = _products.FindIndex(p => p.Name.Equals(oldName, StringComparison.OrdinalIgnoreCase));
    if (index != -1)
    {
        _products[index] = updatedProduct;
    }
    else
    {
        _products.Add(updatedProduct);
    }

    if (!oldName.Equals(updatedProduct.Name, StringComparison.OrdinalIgnoreCase))
    {
        _productsByNameIndex.Remove(oldName);
    }
    
    _productsByNameIndex[updatedProduct.Name] = updatedProduct;
}

    public void Delete(Product product)
    {
        if (product == null) return;

        _products.Remove(product);
        _productsByNameIndex.Remove(product.Name);
    }
}