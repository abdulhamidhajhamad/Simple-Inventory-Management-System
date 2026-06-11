namespace InventorySystem.UI.Screens;

public interface IConsoleScreen
{
    string Key { get; }
    string Description { get; }
    void Execute();
}