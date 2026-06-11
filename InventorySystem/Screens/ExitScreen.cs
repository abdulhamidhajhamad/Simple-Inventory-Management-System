using System;

namespace InventorySystem.UI.Screens;

public class ExitScreen : IConsoleScreen
{
    private readonly Action _shutdownAction;

    public ExitScreen(Action shutdownAction)
    {
        _shutdownAction = shutdownAction;
    }

    public string Key => "5";
    public string Description => "Exit";

    public void Execute()
    {
        Console.WriteLine("Exiting application. Goodbye!");
        _shutdownAction();
    }
}