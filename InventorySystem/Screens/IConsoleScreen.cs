namespace InventorySystem.UI.Screens;

public interface IConsoleScreen
{
    string Description { get; }
    void Execute();
}