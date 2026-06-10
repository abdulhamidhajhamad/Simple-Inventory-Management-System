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

    public Result AddProduct(string name, decimal price, int quantity)
    {
        var productResult = Product.Create(name, price, quantity);
        if (productResult.IsFailure)
            return Result.Failure(productResult.ErrorMessage);

        var existingProduct = _repository.GetByName(name);
        if (existingProduct != null)
            return Result.Failure($"A product named '{name}' already exists in the inventory.");

        _repository.Add(productResult.Value!);
        return Result.Success();
    }
}