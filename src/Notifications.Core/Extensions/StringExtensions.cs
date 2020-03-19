using System;

namespace Notifications.Core.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveAllExceptLettersOrDigits(this string str)
        {
            char[] arr = str.ToCharArray();
            arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c))));
            return new string(arr);
        }

        public static string RemoveAllExceptDigitsOrPoint(this string str)
        {
            char[] arr = str.ToCharArray();
            arr = Array.FindAll<char>(arr, (c => (char.IsDigit(c)) || (c=='.')));
            return new string(arr);
        }
    }
}
