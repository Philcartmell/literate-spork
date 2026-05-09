using System;
using System.Globalization;

namespace Notifications.Consumer
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            double firstNumber = ReadNumber("Enter the first number: ");
            double secondNumber = ReadNumber("Enter the second number: ");

            double sum = firstNumber + secondNumber;
            Console.WriteLine($"The sum is: {sum.ToString(CultureInfo.CurrentCulture)}");
        }

        private static double ReadNumber(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (double.TryParse(input, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out double number))
                {
                    return number;
                }

                Console.WriteLine("Invalid number. Please try again.");
            }
        }
    }
}
