using System;
using InventorySystem.UI; 
namespace InventorySystem.UI.Screens;

public class ExitScreen : IConsoleScreen
{
    private readonly ConsoleUI _ui;

    public ExitScreen(ConsoleUI ui)
    {
        _ui = ui ?? throw new ArgumentNullException(nameof(ui));
    }

    public string Description => "Exit";

    public void Execute()
    {
        Console.WriteLine("Exiting application. Goodbye!");
        _ui.Shutdown();
    }
}