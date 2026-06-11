using System;
using System.Globalization;
using InventorySystem.Services;

namespace InventorySystem.UI.Screens;

public class SearchProductScreen : IConsoleScreen
{
    private readonly IInventoryService _inventoryService;

    public SearchProductScreen(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public string Key => "5";
    public string Description => "Search for a Product";

    public void Execute()
    {
        using (new ConsoleColorContext(ConsoleColor.Cyan))
        {
            Console.WriteLine("--- Search for a Product ---");
        }

        string targetName = ConsoleInput.PromptString("Enter product name to search: ");

        var result = _inventoryService.SearchProductByName(targetName);

        if (result.IsSuccess)
        {
            var product = result.Value!;
            
            using (new ConsoleColorContext(ConsoleColor.Green))
            {
                Console.WriteLine("\n✨ Product Found Successfully!");
            }

            Console.WriteLine("+-------------------------+---------------+---------------+");
            Console.WriteLine("| Product Name            | Price         | Quantity      |");
            Console.WriteLine("+-------------------------+---------------+---------------+");
            
            string nameStr = product.Name.PadRight(24);
            string priceStr = product.Price.ToString("F2", CultureInfo.InvariantCulture).PadRight(14);
            string qtyStr = product.Quantity.ToString().PadRight(14);
            
            Console.WriteLine($"| {nameStr}| {priceStr}| {qtyStr}|");
            Console.WriteLine("+-------------------------+---------------+---------------+");
        }
        else
        {
            using (new ConsoleColorContext(ConsoleColor.Red))
            {
                Console.WriteLine($"\n❌ Error: {result.ErrorMessage}");
            }
        }
    }
}