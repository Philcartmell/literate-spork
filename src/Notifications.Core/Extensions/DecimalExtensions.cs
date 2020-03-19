using System;

namespace Notifications.Core.Extensions
{
    public static class DecimalExtensions
    {
        public static int CountDecimalPlaces(this decimal decimalValue)
        {
            return BitConverter.GetBytes(decimal.GetBits(decimalValue)[3])[2];
        }

        public static decimal GetFraction(this decimal theDecimal)
        {
            decimal d = theDecimal % 1.0m;
            return d;
        }

        public static decimal RoundDown(this decimal theDecimal)
        {
            return Math.Floor(theDecimal * 100) / 100;
        }
    }
}
