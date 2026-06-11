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
        _screens = new Dictionary<string, IConsoleScreen>();
        
        int currentKey = 1;
        foreach (var screen in screens)
        {
            _screens.Add(currentKey.ToString(), screen);
            currentKey++;
        }
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
                int minKey = _screens.Keys.Select(int.Parse).Min();
                int maxKey = _screens.Keys.Select(int.Parse).Max();
                LogError($"Invalid choice. Please enter a value between {minKey}-{maxKey}.");
            }
        }
    }

    private void DisplayMenu()
    {
        Console.WriteLine("=== Inventory Management System ===");
        
        foreach (var pair in _screens.OrderBy(p => int.Parse(p.Key)))
        {
            Console.WriteLine($"{pair.Key}. {pair.Value.Description}");
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