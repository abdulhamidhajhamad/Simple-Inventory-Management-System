using System.Collections.Generic;
using InventorySystem.Common;
using InventorySystem.Domain;
using InventorySystem.Repositories;

namespace InventorySystem.Services;

public class InventoryService : IInventoryService
{
    private readonly IProductRepository _repository;

    public InventoryService(IProductRepository repository)
    {
        _repository = repository;
    }

    public bool IsNameDuplicate(string name)
    {
        return _repository.GetByName(name) != null;
    }

    public Result AddProduct(string name, decimal price, int quantity)
    {
        var productResult = Product.Create(name, price, quantity);
        if (productResult.IsFailure)
return Result.Failure(productResult.ErrorMessage ?? "An unknown error occurred during product creation.");
        if (IsNameDuplicate(name))
            return Result.Failure($"A product named '{name}' already exists in the inventory.");

        _repository.Add(productResult.Value!);
        return Result.Success();
    }

    public Result<IReadOnlyCollection<Product>> GetAllProducts()
    {
        var products = _repository.GetAll();

        if (products.Count == 0)
        {
            return Result<IReadOnlyCollection<Product>>.Failure("The inventory is currently empty. Add some products first!");
        }

        return Result<IReadOnlyCollection<Product>>.Success(products);
    }
}