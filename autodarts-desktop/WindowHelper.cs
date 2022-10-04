using System;


namespace autodarts_desktop
{
    /// <summary>
    /// Provides conversations between data-types.
    /// </summary>
    public static class WindowHelper
    {
        public static string GetStringByBool(bool input, string trueValue = "1", string falseValue = "0")
        {
            return input ? trueValue : falseValue;
        }
        public static bool GetBoolByString(string input, string trueValue = "1")
        {
            return input == trueValue ? true : false;
        }


        public static string GetStringByDouble(double input)
        {
            return Math.Round(input, 2).ToString().Replace(",", ".");
        }

        public static double GetDoubleByString(string str)
        {
            return Double.Parse(str.Replace(".", ","));
        }


    }
}
