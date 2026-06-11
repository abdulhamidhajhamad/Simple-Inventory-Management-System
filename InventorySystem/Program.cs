using System;
using System.Collections.Generic;
using System.Text;
using InventorySystem.Repositories;
using InventorySystem.Services;
using InventorySystem.UI;
using InventorySystem.UI.Screens;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

var repository = new InMemoryProductRepository();
var inventoryService = new InventoryService(repository);

ConsoleUI ui = null!;

var screens = new List<IConsoleScreen>
{
    new AddProductScreen(inventoryService),
    new ViewProductsScreen(inventoryService),
    new EditProductScreen(inventoryService),
    new DeleteProductScreen(inventoryService),
    new SearchProductScreen(inventoryService), 
    new ExitScreen(() => ui.Shutdown())     
};

ui = new ConsoleUI(screens);
ui.Run();