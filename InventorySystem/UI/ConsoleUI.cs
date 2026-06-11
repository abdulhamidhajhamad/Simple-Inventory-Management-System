using System;
using System.Collections.Generic;
using System.Linq; // ← تأكد من إضافة الـ Linq لحساب العرض ديناميكياً
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
        
        // التحديث السينيور للـ Registry بدون switch-case:
        _menuRegistry = new() 
        {
            { "1", ("Add a Product", HandleAddProduct) },
            { "2", ("View All Products", HandleViewProducts) }, 
            { "3", ("Exit", HandleExit) }                       
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
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Invalid choice. Try again.");
                Console.ResetColor();
            }

            Console.WriteLine(); 
        }
    }

    private void DisplayMenu()
    {
        Console.WriteLine("=== Simple Inventory Management System ===");
        foreach (var item in _menuRegistry)
        {
            Console.WriteLine($"{item.Key}. {item.Value.Description}");
        }
        Console.Write("Select an option: ");
    }

    private void HandleAddProduct()
    {
        Console.WriteLine("--- Add New Product ---");

        string name = ConsoleInput.PromptString("Enter product name: ");
        if (_inventoryService.IsNameDuplicate(name))
        {
            LogError($"A product named '{name}' already exists. Operation aborted.");
            return; 
        }

        decimal price = ConsoleInput.PromptDecimal("Enter product price: ");
        if (!InventorySystem.Domain.Product.IsValidPrice(price))
        {
            LogError("Price cannot be negative. Operation aborted.");
            return;
        }

        int quantity = ConsoleInput.PromptInt("Enter product quantity: ");
        if (!InventorySystem.Domain.Product.IsValidQuantity(quantity))
        {
            LogError("Quantity cannot be negative. Operation aborted.");
            return;
        }

        var result = _inventoryService.AddProduct(name, price, quantity);

        if (result.IsSuccess)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nSuccess: Product added successfully!");
        }
        else
        {
            LogError(result.ErrorMessage ?? "Failed to add product due to an unexpected error.");
        }
        Console.ResetColor();
    }


    private void HandleViewProducts()
    {
        Console.WriteLine("--- All Inventory Products ---");

        var result = _inventoryService.GetAllProducts();

    if (!result.IsSuccess)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\nInfo: {result.ErrorMessage ?? "No products found."}");
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
            columnWidths[0] = Math.Max(columnWidths[0], p.Name?.Length ?? 0);
            columnWidths[1] = Math.Max(columnWidths[1], $"{p.Price:F2}".Length);
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
            string priceStr = $"{p.Price:F2}".PadRight(columnWidths[1] - 1);
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
        Console.WriteLine($"\nError: {message}");
        Console.ResetColor();
    }

    private void HandleExit()
    {
        Console.WriteLine("Exiting application. Goodbye!");
        _isRunning = false;
    }
}