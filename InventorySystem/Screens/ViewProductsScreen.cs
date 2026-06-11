using System;
using System.Globalization;
using System.Linq;
using InventorySystem.Services;

namespace InventorySystem.UI.Screens;

public class ViewProductsScreen : IConsoleScreen
{
    private readonly IInventoryService _inventoryService;

    public ViewProductsScreen(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public string Description => "View All Products";

    public void Execute()
    {
        var result = _inventoryService.GetAllProducts();

        if (result.IsFailure)
        {
            using (new ConsoleColorContext(ConsoleColor.Red))
            {
                Console.WriteLine($"Error: {result.ErrorMessage}");
            }
            return;
        }

        var products = result.Value!;
        int[] columnWidths = { 25, 15, 15 };
        string rowDivider = $"+{new string('-', columnWidths[0])}+{new string('-', columnWidths[1])}+{new string('-', columnWidths[2])}+";

        using (new ConsoleColorContext(ConsoleColor.Yellow))
        {
            Console.WriteLine(rowDivider);
            Console.WriteLine($"| {"Product Name".PadRight(columnWidths[0] - 1)}| {"Price".PadRight(columnWidths[1] - 1)}| {"Quantity".PadRight(columnWidths[2] - 1)}|");
            Console.WriteLine(rowDivider);
        }

        foreach (var p in products)
        {
            string nameStr = p.Name.PadRight(columnWidths[0] - 1);
            string priceStr = p.Price.ToString("F2", CultureInfo.InvariantCulture).PadRight(columnWidths[1] - 1);
            string qtyStr = p.Quantity.ToString().PadRight(columnWidths[2] - 1);

            Console.WriteLine($"| {nameStr}| {priceStr}| {qtyStr}|");
        }

        Console.WriteLine(rowDivider);
        
        using (new ConsoleColorContext(ConsoleColor.DarkGray))
        {
            Console.WriteLine($"Total Product Types: {products.Count}");
        }
    }
}