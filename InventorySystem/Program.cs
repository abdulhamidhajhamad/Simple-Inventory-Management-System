using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventorySystem.Repositories;
using InventorySystem.Services;
using InventorySystem.UI;
using InventorySystem.UI.Screens;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

var repository = new InMemoryProductRepository();
var inventoryService = new InventoryService(repository);

var ui = new ConsoleUI();

var screenInterfaceType = typeof(IConsoleScreen);
var assembly = typeof(Program).Assembly; 

var screenTypes = assembly.GetTypes()
    .Where(t => screenInterfaceType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

var screensList = new List<IConsoleScreen>();

foreach (var type in screenTypes)
{
    IConsoleScreen screenInstance;

    if (type == typeof(ExitScreen))
    {
        screenInstance = (IConsoleScreen)Activator.CreateInstance(type, ui)!;
    }
    else
    {
        screenInstance = (IConsoleScreen)Activator.CreateInstance(type, inventoryService)!;
    }

    screensList.Add(screenInstance);
}

var orderedScreens = screensList
    .OrderBy(s => s is ExitScreen) 
    .ToList();

ui.SetScreens(orderedScreens);

ui.Run();