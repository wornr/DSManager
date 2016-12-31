using System;
using System.Globalization;
using DSManager.Model.Enums;

namespace DSManager.Utilities {
    public static class ValueToText {
        private static readonly string[] Unities = { "jeden", "dwa", "trzy", "cztery", "pięć", "sześć", "siedem", "osiem", "dziewięć" };
        private static readonly string[] DozensUnities = { "jedenaście", "dwanaście", "trzynaście", "czternaście", "piętnaście", "szesnaście", "siedemnaście", "osiemnaście", "dziewiętnaście" };
        private static readonly string[] Dozens = { "dziesięć", "dwadzieścia", "trzydzieści", "czterdzieści", "pięćdziesiąt", "szesćdziesiąt", "siedemdziesiąt", "osiemdziesiąt", "dziewięćdziesiąt" };
        private static readonly string[] Hundreds = { "sto", "dwieście", "trzysta", "czterysta", "pięćset", "szesćset", "siedemset", "osiemset", "dziewięćset" };
        private static readonly string[,] Groups = {{"tysiąc", "tysiące", "tysięcy"},
                {"milion", "miliony", "milionów"},
                {"miliard", "miliardy", "miliardów"},
                {"bilion", "biliony", "bilionów"},
                {"biliard", "biliardy", "biliardów"},
                {"trylion", "tryliardy", "tryliardów"}
            };
        private static readonly string[,] TotalCurrencies = {{"złoty", "złote", "złotych"},
                {"euro", "euro", "euro"},
                {"dolar", "dolary", "dolarów"},
                {"funt", "funty", "funtów"}
            };
        private static readonly string[,] DecimalCurrencies = {{"grosz", "grosze", "groszy"},
                {"cent", "centy", "centów"},
                {"cent", "centy", "centów"},
                {"pens", "pensy", "pensów"}
            };

        private const string Sign = "minus";

        private static string Parse(long value, bool groupsOnly = true) {
            var result = ""; // result

            var g = 0;

            while(value != 0) {
                var h = (int)value % 1000 / 100;
                var d = (int)value % 100 / 10;
                var u = (int)value % 10;

                int shift;
                if(h == 0 && d == 0 && u == 0) {
                    shift = g;
                    g = 0;
                } else {
                    shift = 0;
                }

                int dU;
                if(d == 1 && u != 0) {
                    dU = u;
                    d = 0;
                    u = 0;
                } else {
                    dU = 0;
                }

                int e;
                if(u == 1 && h == 0 && d == 0 && g > 0) {
                    e = 0;
                    if(groupsOnly)
                        u = 0;
                } else if(u > 1 && u < 5) {
                    e = 1;
                } else {
                    e = 2;
                }

                result = (h == 0 ? "" : Hundreds[h - 1] + " ") +
                    (d == 0 ? "" : Dozens[d - 1] + " ") +
                    (dU == 0 ? "" : DozensUnities[dU - 1] + " ") +
                    (u == 0 ? "" : Unities[u - 1] + " ") +
                    (g == 0 ? "" : Groups[g - 1, e] + " ") +
                    result;

                g += 1 + shift;
                value /= 1000;
            }

            return result;
        }

        /// <summary>
        /// Translates numeric value to it's text representation
        /// </summary>
        /// <param name="value">numeric value of type decimal</param>
        /// <param name="currency">Currency type which has to be displayed.</param>
        /// <param name="groupsOnly">If true 1000 => thousand otherwise 1000 => one thousand.</param>
        /// <returns>Text representation of given value with/without currency depending of currency param</returns>
        public static string Translate(decimal value, Currency currency = Currency.None, bool groupsOnly = true) {
            string totalCurrency = "", decimalCurrency = "";
            var showSign = false;

            var values = value.ToString(CultureInfo.InvariantCulture).Split('.');

            var totalPart = long.Parse(values[0]);     // total part of number
            var decimalPart = 0L;                      // decimal part of number
            if(values.Length == 2) {
                if(values[1].Length == 1)
                    values[1] = values[1] + "0";
                else if(values[1].Length > 2)
                    values[1] = values[1].Substring(0, 2);

                decimalPart = long.Parse(values[1]);
            }

            if(totalPart < 0) {
                showSign = true;
                totalPart = Math.Abs(totalPart);
            }

            if(currency != Currency.None) {
                if(totalPart == 1) {
                    totalCurrency = TotalCurrencies[(int)currency - 1, 0];
                } else if(totalPart > 1 && totalPart < 5) {
                    totalCurrency = TotalCurrencies[(int)currency - 1, 1];
                } else {
                    totalCurrency = TotalCurrencies[(int)currency - 1, 2];
                }

                if(decimalPart == 1) {
                    decimalCurrency = DecimalCurrencies[(int)currency - 1, 0];
                } else if(decimalPart > 1 && decimalPart < 5) {
                    decimalCurrency = DecimalCurrencies[(int)currency - 1, 1];
                } else {
                    decimalCurrency = DecimalCurrencies[(int)currency - 1, 2];
                }
            }

            if(values.Length == 2)
                return (showSign ? Sign + " " : "") + Parse(totalPart, groupsOnly) + totalCurrency + " " + Parse(decimalPart, groupsOnly) + decimalCurrency;
            
            return (showSign ? Sign + " " : "") + Parse(totalPart, groupsOnly) + totalCurrency;
        }
    }
}
