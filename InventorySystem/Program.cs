using System.Collections.Generic;
using InventorySystem.Repositories;
using InventorySystem.Services;
using InventorySystem.UI;
using InventorySystem.UI.Screens;

var repository = new InMemoryProductRepository();
var inventoryService = new InventoryService(repository);

ConsoleUI ui = null!;

var screens = new List<IConsoleScreen>
{
    new AddProductScreen(inventoryService),
    new ViewProductsScreen(inventoryService),
    new EditProductScreen(inventoryService),
    new DeleteProductScreen(inventoryService),
    new ExitScreen(() => ui.Shutdown())
};

ui = new ConsoleUI(screens);

ui.Run();