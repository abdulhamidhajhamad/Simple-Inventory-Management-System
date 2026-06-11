using System;
using InventorySystem.Repositories;
using InventorySystem.Services;
using InventorySystem.UI;

namespace InventorySystem;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.InputEncoding = System.Text.Encoding.UTF8;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Title = "Advanced Inventory Management System";

        IProductRepository repository = new InMemoryProductRepository();

        IInventoryService inventoryService = new InventoryService(repository);

        ConsoleUI ui = new ConsoleUI(inventoryService);

        ui.Run();
    }
}