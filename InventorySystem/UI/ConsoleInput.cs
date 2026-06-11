using System;
using InventorySystem.Common; 

namespace InventorySystem.UI;

public static class ConsoleInput
{
    public static string PromptString(string message, Func<string, Result>? inlineValidator = null)
    {
        while (true)
        {
            Console.Write(message);
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input cannot be empty.");
                continue;
            }

            string cleanInput = input.Trim();

            if (inlineValidator != null)
            {
                var validationResult = inlineValidator(cleanInput);
                if (validationResult.IsFailure)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: {validationResult.ErrorMessage}");
                    Console.ResetColor();
                    continue; 
                }
            }

            return cleanInput;
        }
    }

    public static decimal PromptDecimal(string message, Func<decimal, Result>? inlineValidator = null)
    {
        while (true)
        {
            Console.Write(message);
            if (!decimal.TryParse(Console.ReadLine(), out decimal result))
            {
                Console.WriteLine("Invalid format. Please enter a valid decimal number.");
                continue;
            }

            if (inlineValidator != null)
            {
                var validationResult = inlineValidator(result);
                if (validationResult.IsFailure)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: {validationResult.ErrorMessage}");
                    Console.ResetColor();
                    continue;
                }
            }

            return result;
        }
    }

    public static int PromptInt(string message, Func<int, Result>? inlineValidator = null)
    {
        while (true)
        {
            Console.Write(message);
            if (!int.TryParse(Console.ReadLine(), out int result))
            {
                Console.WriteLine("Invalid format. Please enter a valid whole number.");
                continue;
            }

            if (inlineValidator != null)
            {
                var validationResult = inlineValidator(result);
                if (validationResult.IsFailure)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: {validationResult.ErrorMessage}");
                    Console.ResetColor();
                    continue;
                }
            }

            return result;
        }
    }
}