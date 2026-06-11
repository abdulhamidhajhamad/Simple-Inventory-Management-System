using System;
using InventorySystem.Services;
using InventorySystem.Common;

namespace InventorySystem.UI.Screens;

public class EditProductScreen : IConsoleScreen
{
    private readonly IInventoryService _inventoryService;

    public EditProductScreen(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public string Key => "3";
    public string Description => "Edit a Product";

    public void Execute()
    {
        using (new ConsoleColorContext(ConsoleColor.Cyan))
        {
            Console.WriteLine("--- Edit Existing Product ---");
        }

        string targetName = ConsoleInput.PromptString("Enter the name of the product to update: ", nameInput =>
        {
            var searchResult = _inventoryService.GetProductForUpdate(nameInput);
            if (searchResult.IsFailure)
                return Result.Failure(searchResult.ErrorMessage!);
            return Result.Success();
        });

        var currentProduct = _inventoryService.GetProductForUpdate(targetName).Value!;
        Console.WriteLine($"\nCurrent Data -> Name: {currentProduct.Name} | Price: {currentProduct.Price:F2} | Qty: {currentProduct.Quantity}");
        Console.WriteLine("Enter the new details below:\n");

        string newName = ConsoleInput.PromptString("Enter new product name: ", inputName => 
        {
            if (!targetName.Equals(inputName, StringComparison.OrdinalIgnoreCase) && _inventoryService.IsNameDuplicate(inputName))
                return Result.Failure($"A product named '{inputName}' already exists.");        
            return Result.Success();
        });

        decimal newPrice = ConsoleInput.PromptDecimal("Enter new product price: ", inputPrice =>
        {
            if (!InventorySystem.Domain.Product.IsValidPrice(inputPrice))
                return Result.Failure("Price must be greater than zero.");
            return Result.Success();
        });

        int newQuantity = ConsoleInput.PromptInt("Enter new product quantity: ", inputQty =>
        {
            if (!InventorySystem.Domain.Product.IsValidQuantity(inputQty))
                return Result.Failure("Quantity cannot be negative.");
            return Result.Success();
        });

        var result = _inventoryService.UpdateProduct(targetName, newName, newPrice, newQuantity);

        if (result.IsSuccess)
        {
            using (new ConsoleColorContext(ConsoleColor.Green))
            {
                Console.WriteLine("\nProduct updated successfully.");
            }
        }
        else
        {
            using (new ConsoleColorContext(ConsoleColor.Red))
            {
                Console.WriteLine($"\nError: {result.ErrorMessage ?? "An unexpected error occurred."}");
            }
        }
    }
}