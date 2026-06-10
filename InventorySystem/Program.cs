using System;
using InventorySystem.Repositories;
using InventorySystem.Services;
using InventorySystem.UI;

namespace InventorySystem;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.Title = "Advanced Inventory Management System - Phase 1";

        IProductRepository repository = new InMemoryProductRepository();

        IInventoryService inventoryService = new InventoryService(repository);

        ConsoleUI ui = new ConsoleUI(inventoryService);

        ui.Run();
    }
}