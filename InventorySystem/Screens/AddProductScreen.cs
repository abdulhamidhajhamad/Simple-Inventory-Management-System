using System;
using InventorySystem.Services;

namespace InventorySystem.UI.Screens;

public class AddProductScreen : IConsoleScreen
{
    private readonly IInventoryService _inventoryService;

    public AddProductScreen(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public string Key => "1";
    public string Description => "Add a Product";

    public void Execute()
    {
        using (new ConsoleColorContext(ConsoleColor.Cyan))
        {
            Console.WriteLine("--- Add a New Product ---");
        }

        string name = ConsoleInput.PromptString("Enter product name: ");

        decimal price = ConsoleInput.PromptDecimal("Enter product price: ", inputPrice =>
        {
            if (!InventorySystem.Domain.Product.IsValidPrice(inputPrice))
                return InventorySystem.Common.Result.Failure("Price must be greater than zero.");
            return InventorySystem.Common.Result.Success();
        });

        int quantity = ConsoleInput.PromptInt("Enter product quantity: ", inputQty =>
        {
            if (!InventorySystem.Domain.Product.IsValidQuantity(inputQty))
                return InventorySystem.Common.Result.Failure("Quantity cannot be negative.");
            return InventorySystem.Common.Result.Success();
        });

        var result = _inventoryService.AddProduct(name, price, quantity);

        if (result.IsSuccess)
        {
            using (new ConsoleColorContext(ConsoleColor.Green))
            {
                Console.WriteLine("\nProduct added successfully.");
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