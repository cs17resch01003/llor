namespace LLOR.TestRunner
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ExtensionMethods
    {
        public static string Replace(this string input, string oldValue, int newValue)
        {
            return input.Replace(oldValue, $"${newValue}$");
        }

        public static string Replace(this string input, string oldValue, int newValue, string singular, string plural)
        {
            string temp = $"${newValue}$ {(newValue == 1 ? singular : plural)}"; 
            return input.Replace(oldValue, temp);
        }

        public static string Replace(this string input, string oldValue, double newValue, int? precision = null)
        {
            if (precision == null)
                return input.Replace(oldValue, Convert.ToInt32(Math.Round(newValue)));
            return input.Replace(oldValue, Math.Round(newValue, precision.Value).ToString("0.00"));
        }

        public static string Replace<T>(this string input, string oldValue, IEnumerable<T> newValue)
        {
            return input.Replace(oldValue, newValue.Count());
        }

        public static string Replace<T>(this string input, string oldValue, IEnumerable<T> newValue, string singular, string plural)
        {
            return input.Replace(oldValue, newValue.Count(), singular, plural);
        }
    }
}
