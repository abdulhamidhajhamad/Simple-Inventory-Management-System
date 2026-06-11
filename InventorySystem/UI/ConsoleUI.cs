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
            { "3", ("Exit", HandleExit) }                       
        };
    }

public void Run()
{
    // تنظيف الشاشة لمرة واحدة فقط عند تشغيل البرنامج أول مرة
    Console.Clear(); 

    while (_isRunning)
    {
        DisplayMenu();
        
        string? choice = Console.ReadLine()?.Trim();
        Console.WriteLine(); // سطر فارغ لتنسيق بداية الأكشن

        if (choice != null && _menuRegistry.TryGetValue(choice, out var option))
        {
            option.Action();
            
            // 🔥 حذفت الـ ReadKey والرسالة المزعجة من هنا!
            Console.WriteLine();
            Console.WriteLine();
        }
        else
        {
            LogError("Invalid choice. Please select a valid option from the menu (1-3).");
            Console.WriteLine(); 
        }
    }
}

    private void DisplayMenu()
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("==================================================");
        Console.WriteLine("    📦 SIMPLE INVENTORY MANAGEMENT SYSTEM         ");
        Console.WriteLine("==================================================");
        
        foreach (var item in _menuRegistry)
        {
            Console.WriteLine($" [{item.Key}] {item.Value.Description}");
        }
        
        Console.WriteLine("--------------------------------------------------");
        Console.Write("👉 Select an option: ");
        Console.ResetColor();
    }

    private void HandleAddProduct()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("--- ➕ Add New Product ---");
        Console.ResetColor();

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
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n✨ Success: Product added successfully!");
        }
        else
        {
            LogError(result.ErrorMessage ?? "Failed to add product due to an unexpected error.");
        }
        Console.ResetColor();
    }

    private void HandleViewProducts()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("--- 📋 All Inventory Products ---");
        Console.ResetColor();

        var result = _inventoryService.GetAllProducts();

        if (!result.IsSuccess)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n💡 Info: {result.ErrorMessage ?? "No products found."}");
            Console.ResetColor();
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

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(rowDivider);
        Console.WriteLine($"| {headers[0].PadRight(columnWidths[0] - 1)}| {headers[1].PadRight(columnWidths[1] - 1)}| {headers[2].PadRight(columnWidths[2] - 1)}|");
        Console.WriteLine(rowDivider);
        Console.ResetColor();

        foreach (var p in products)
        {
            string nameStr = p.Name.PadRight(columnWidths[0] - 1);
            string priceStr = p.Price.ToString("F2", CultureInfo.InvariantCulture).PadRight(columnWidths[1] - 1);
            string qtyStr = p.Quantity.ToString().PadRight(columnWidths[2] - 1);

            Console.WriteLine($"| {nameStr}| {priceStr}| {qtyStr}|");
        }

        Console.WriteLine(rowDivider);
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"📊 Total Product Types: {products.Count}");
        Console.ResetColor();
    }

    private static void LogError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"❌ Error: {message}");
        Console.ResetColor();
        Console.WriteLine();
    }

    private void HandleExit()
    {
        Console.WriteLine("Exiting application. Goodbye!");
        _isRunning = false;
    }
}