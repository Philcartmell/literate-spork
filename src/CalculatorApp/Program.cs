using System;
using Spectre.Console;

namespace CalculatorApp
{
    internal static class Program
    {
        private static void Main()
        {
            AnsiConsole.MarkupLine("[green]Welcome to the console calculator![/]");

            while (true)
            {
                decimal left = ReadNumber("Enter the first number");
                decimal right = ReadNumber("Enter the second number");
                char operation = ReadOperation();

                try
                {
                    decimal result = Calculator.Calculate(left, right, operation);
                    AnsiConsole.MarkupLine($"[yellow]{left} {operation} {right} = {result}[/]");
                }
                catch (DivideByZeroException ex)
                {
                    AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
                    continue;
                }

                if (!AnsiConsole.Confirm("Perform another calculation?", false))
                {
                    break;
                }
            }

            AnsiConsole.MarkupLine("[green]Goodbye![/]");
        }

        private static decimal ReadNumber(string prompt)
        {
            while (true)
            {
                string input = AnsiConsole.Ask<string>(prompt + ":");

                if (decimal.TryParse(input, out decimal number))
                {
                    return number;
                }

                AnsiConsole.MarkupLine("[red]Please enter a valid numeric value.[/]");
            }
        }

        private static char ReadOperation()
        {
            while (true)
            {
                string input = AnsiConsole.Ask<string>("Enter an operator (+, -, *, /):");

                if (!string.IsNullOrWhiteSpace(input) && input.Length == 1)
                {
                    char operation = input[0];
                    if (operation == '+' || operation == '-' || operation == '*' || operation == '/')
                    {
                        return operation;
                    }
                }

                AnsiConsole.MarkupLine("[red]Please enter one of the supported operators: +, -, *, /.[/]");
            }
        }
    }
}
