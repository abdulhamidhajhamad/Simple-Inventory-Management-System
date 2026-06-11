using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using InventorySystem.Services;
using InventorySystem.Domain;
using InventorySystem.Common;

namespace InventorySystem.UI;

public class ConsoleUI
{
    private readonly IInventoryService _inventoryService;
    private readonly Dictionary<string, (string Description, Action Action)> _menuRegistry;
    private bool _isRunning = true;

    public ConsoleUI(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;

        _menuRegistry = new()
{
    { "1", ("Add a Product", HandleAddProduct) },
    { "2", ("View All Products", HandleViewProducts) },
    { "3", ("Edit a Product", HandleEditProduct) },
    { "4", ("Delete a Product", HandleDeleteProduct) },
    { "5", ("Exit", HandleExit) }
};
    }

    public void Run()
    {
        Console.Clear();

        while (_isRunning)
        {
            DisplayMenu();

            string? choice = Console.ReadLine()?.Trim();
            Console.WriteLine();

            if (choice != null && _menuRegistry.TryGetValue(choice, out var option))
            {
                option.Action();

                Console.WriteLine();
                Console.WriteLine();
            }
            else
            {
                string validRange = string.Join("-", _menuRegistry.Keys.Min(), _menuRegistry.Keys.Max());
                LogError($"Invalid choice. Please select a valid option from the menu ({validRange}).");
                Console.WriteLine();
            }
        }
    }

    private void DisplayMenu()
    {
        using (new ConsoleColorContext(ConsoleColor.Gray))
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("    📦 SIMPLE INVENTORY MANAGEMENT SYSTEM         ");
            Console.WriteLine("==================================================");

            foreach (var item in _menuRegistry)
            {
                Console.WriteLine($" [{item.Key}] {item.Value.Description}");
            }

            Console.WriteLine("--------------------------------------------------");
            Console.Write("👉 Select an option: ");
        }
    }

    private void HandleAddProduct()
    {
        using (new ConsoleColorContext(ConsoleColor.Cyan))
        {
            Console.WriteLine("--- ➕ Add New Product ---");
        }

        string name = ConsoleInput.PromptString("Enter product name: ", inputName =>
        {
            if (_inventoryService.IsNameDuplicate(inputName))
                return Result.Failure($"A product named '{inputName}' already exists in the inventory.");
            return Result.Success();
        });

        decimal price = ConsoleInput.PromptDecimal("Enter product price: ", inputPrice =>
        {
            if (!InventorySystem.Domain.Product.IsValidPrice(inputPrice))
                return Result.Failure("Price must be greater than zero.");
            return Result.Success();
        });

        int quantity = ConsoleInput.PromptInt("Enter product quantity: ", inputQty =>
        {
            if (!InventorySystem.Domain.Product.IsValidQuantity(inputQty))
                return Result.Failure("Quantity cannot be negative.");
            return Result.Success();
        });

        var result = _inventoryService.AddProduct(name, price, quantity);

        if (result.IsSuccess)
        {
            using (new ConsoleColorContext(ConsoleColor.Green))
            {
                Console.WriteLine("\n✨ Success: Product added successfully!");
            }
        }
        else
        {
            LogError(result.ErrorMessage ?? "Failed to add product due to an unexpected error.");
        }
    }

    private void HandleViewProducts()
    {
        using (new ConsoleColorContext(ConsoleColor.Cyan))
        {
            Console.WriteLine("--- 📋 All Inventory Products ---");
        }

        var result = _inventoryService.GetAllProducts();

        if (!result.IsSuccess)
        {
            using (new ConsoleColorContext(ConsoleColor.Yellow))
            {
                Console.WriteLine($"\n💡 Info: {result.ErrorMessage ?? "No products found."}");
            }
            return;
        }

        RenderTable(result.Value!);
    }

    private static void RenderTable(IReadOnlyCollection<Product> products)
    {
        string[] headers = { "Product Name", "Price", "Quantity" };
        int[] columnWidths = new int[3];

        for (int i = 0; i < headers.Length; i++)
            columnWidths[i] = headers[i].Length;

        foreach (var p in products)
        {
            columnWidths[0] = Math.Max(columnWidths[0], p.Name.Length);
            columnWidths[1] = Math.Max(columnWidths[1], p.Price.ToString("F2", CultureInfo.InvariantCulture).Length);
            columnWidths[2] = Math.Max(columnWidths[2], p.Quantity.ToString().Length);
        }

        for (int i = 0; i < columnWidths.Length; i++)
            columnWidths[i] += 4;

        string rowDivider = "+" + string.Join("+", columnWidths.Select(w => new string('-', w))) + "+";

        using (new ConsoleColorContext(ConsoleColor.Cyan))
        {
            Console.WriteLine(rowDivider);
            Console.WriteLine($"| {headers[0].PadRight(columnWidths[0] - 1)}| {headers[1].PadRight(columnWidths[1] - 1)}| {headers[2].PadRight(columnWidths[2] - 1)}|");
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
            Console.WriteLine($"📊 Total Product Types: {products.Count}");
        }
    }

    private static void LogError(string message)
    {
        using (new ConsoleColorContext(ConsoleColor.Red))
        {
            Console.Write($"❌ Error: {message}");
        }
        Console.WriteLine();
    }

    private void HandleExit()
    {
        Console.WriteLine("Exiting application. Goodbye!");
        _isRunning = false;
    }

    private void HandleEditProduct()
    {
        using (new ConsoleColorContext(ConsoleColor.Cyan))
        {
            Console.WriteLine("--- 📝 Edit/Update Existing Product ---");
        }

        string targetName = ConsoleInput.PromptString("Enter the name of the product to update: ", nameInput =>
        {
            var searchResult = _inventoryService.GetProductForUpdate(nameInput);
            if (searchResult.IsFailure)
                return Result.Failure(searchResult.ErrorMessage!);
            return Result.Success();
        });

        var currentProduct = _inventoryService.GetProductForUpdate(targetName).Value!;
        Console.WriteLine($"\n💡 Current Data -> Name: {currentProduct.Name} | Price: {currentProduct.Price:F2} | Qty: {currentProduct.Quantity}");
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
                Console.WriteLine("\n✨ Success: Product updated successfully!");
            }
        }
        else
        {
            LogError(result.ErrorMessage ?? "Failed to update product due to an unexpected error.");
        }
    }

    private void HandleDeleteProduct()
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
            LogError(result.ErrorMessage ?? "An unexpected error occurred while deleting the product.");
        }
    }
    private sealed class ConsoleColorContext : IDisposable
    {
        private readonly ConsoleColor _previousColor;

        public ConsoleColorContext(ConsoleColor foregroundColor)
        {
            _previousColor = Console.ForegroundColor;
            Console.ForegroundColor = foregroundColor;
        }

        public void Dispose()
        {
            Console.ForegroundColor = _previousColor;
        }
    }
}