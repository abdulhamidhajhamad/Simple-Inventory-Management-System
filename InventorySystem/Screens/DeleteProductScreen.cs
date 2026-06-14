using System;
using InventorySystem.Services;
using InventorySystem.Common;

namespace InventorySystem.UI.Screens;

public class DeleteProductScreen : IConsoleScreen
{
    private readonly IInventoryService _inventoryService;

    public DeleteProductScreen(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public string Description => "Delete a Product";

    public void Execute()
    {
        using (new ConsoleColorContext(ConsoleColor.Cyan))
        {
            Console.WriteLine("--- Delete Existing Product ---");
        }

        string targetName = ConsoleInput.PromptString("Enter the name of the product to delete: ", nameInput =>
        {
            var productResult = _inventoryService.GetProductForUpdate(nameInput);
            if (productResult.IsFailure)
            {
                return Result.Failure(productResult.ErrorMessage!);
            }
            return Result.Success();
        });

        var result = _inventoryService.DeleteProduct(targetName);

        if (result.IsSuccess)
        {
            using (new ConsoleColorContext(ConsoleColor.Green))
            {
                Console.WriteLine("\nProduct was deleted successfully.");
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