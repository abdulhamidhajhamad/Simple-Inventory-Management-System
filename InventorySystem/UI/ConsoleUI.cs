using System;
using System.Collections.Generic;
using InventorySystem.Services;

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
            { "2", ("Exit", HandleExit) }
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
        decimal price = ConsoleInput.PromptDecimal("Enter product price: ");
        int quantity = ConsoleInput.PromptInt("Enter product quantity: ");

        var result = _inventoryService.AddProduct(name, price, quantity);

        if (result.IsSuccess)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nSuccess: Product added successfully!");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nError: {result.ErrorMessage}");
        }
        Console.ResetColor();
    }

    private void HandleExit()
    {
        Console.WriteLine("Exiting application. Goodbye!");
        _isRunning = false;
    }
}