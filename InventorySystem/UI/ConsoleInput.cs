using System;

namespace InventorySystem.UI;

public static class ConsoleInput
{
    public static string PromptString(string message)
    {
        while (true)
        {
            Console.Write(message);
            string? input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
                return input.Trim();

            Console.WriteLine("Input cannot be empty.");
        }
    }

    public static decimal PromptDecimal(string message)
    {
        while (true)
        {
            Console.Write(message);
            if (decimal.TryParse(Console.ReadLine(), out decimal result))
                return result;

            Console.WriteLine("Invalid format. Please enter a valid decimal number.");
        }
    }

    public static int PromptInt(string message)
    {
        while (true)
        {
            Console.Write(message);
            if (int.TryParse(Console.ReadLine(), out int result))
                return result;

            Console.WriteLine("Invalid format. Please enter a valid whole number.");
        }
    }
}