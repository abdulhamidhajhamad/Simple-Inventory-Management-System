using System;
using System.Collections.Generic;
using System.Linq;
using InventorySystem.UI.Screens;

namespace InventorySystem.UI;

public class ConsoleUI
{
    private readonly Dictionary<string, IConsoleScreen> _screens;
    private bool _isRunning = true;

    public ConsoleUI(IEnumerable<IConsoleScreen> screens)
    {
        _screens = screens.ToDictionary(s => s.Key, s => s);
    }

    public void Run()
    {
        Console.Clear(); 

        while (_isRunning)
        {
            DisplayMenu();
            
            string? choice = Console.ReadLine()?.Trim();
            Console.WriteLine(); 

            if (choice != null && _screens.TryGetValue(choice, out var screen))
            {
                screen.Execute();
                Console.WriteLine();
                Console.WriteLine();
            }
            else
            {
                string validRange = string.Join("-", _screens.Keys.Min(), _screens.Keys.Max());
                LogError($"Invalid choice. Please enter a value between {validRange}.");
            }
        }
    }

    private void DisplayMenu()
    {
        Console.WriteLine("=== Inventory Management System ===");
        foreach (var screen in _screens.Values.OrderBy(s => s.Key))
        {
            Console.WriteLine($"{screen.Key}. {screen.Description}");
        }
        Console.Write("Select an option: ");
    }

    private static void LogError(string message)
    {
        using (new ConsoleColorContext(ConsoleColor.Red))
        {
            Console.Write($"Error: {message}");
        }
        Console.WriteLine();
    }

    public void Shutdown()
    {
        _isRunning = false;
    }
}