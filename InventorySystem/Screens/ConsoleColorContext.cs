using System;

namespace InventorySystem.UI;

public sealed class ConsoleColorContext : IDisposable
{
    private readonly ConsoleColor _previousColor;

    public ConsoleColorContext(ConsoleColor foregroundColor)
    {
        _previousColor = Console.ForegroundColor;
        Console.ForegroundColor = foregroundColor;
    }

    public void Dispose()
    {
        Console.ForegroundColor = _previousColor;
    }
}