using System;

namespace CalculatorApp
{
    internal static class Calculator
    {
        public static decimal Calculate(decimal left, decimal right, char operation)
        {
            switch (operation)
            {
                case '+':
                    return left + right;
                case '-':
                    return left - right;
                case '*':
                    return left * right;
                case '/':
                    if (right == 0)
                    {
                        throw new DivideByZeroException("Cannot divide by zero.");
                    }

                    return left / right;
                default:
                    throw new ArgumentException("Unsupported operator.", nameof(operation));
            }
        }
    }
}
