using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DSManager.Model.Enums;

namespace DSManager.Utilities {
    public static class ValueToText {
        private static string[] unities = { "jeden", "dwa", "trzy", "cztery", "pięć", "sześć", "siedem", "osiem", "dziewięć" };
        private static string[] dozensUnities = { "jedenaście", "dwanaście", "trzynaście", "czternaście", "piętnaście", "szesnaście", "siedemnaście", "osiemnaście", "dziewiętnaście" };
        private static string[] dozens = { "dziesięć", "dwadzieścia", "trzydzieści", "czterdzieści", "pięćdziesiąt", "szesćdziesiąt", "siedemdziesiąt", "osiemdziesiąt", "dziewięćdziesiąt" };
        private static string[] hundreds = { "sto", "dwieście", "trzysta", "czterysta", "pięćset", "szesćset", "siedemset", "osiemset", "dziewięćset" };
        private static string[,] groups = {{"tysiąc", "tysiące", "tysięcy"},
                {"milion", "miliony", "milionów"},
                {"miliard", "miliardy", "miliardów"},
                {"bilion", "biliony", "bilionów"},
                {"biliard", "biliardy", "biliardów"},
                {"trylion", "tryliardy", "tryliardów"}
            };
        private static string[,] totalCurrencies = {{"złoty", "złote", "złotych"},
                {"euro", "euro", "euro"},
                {"dolar", "dolary", "dolarów"},
                {"funt", "funty", "funtów"}
            };
        private static string[,] decimalCurrencies = {{"grosz", "grosze", "groszy"},
                {"cent", "centy", "centów"},
                {"cent", "centy", "centów"},
                {"pens", "pensy", "pensów"}
            };
        private static string sign = "minus";

        private static string parse(long value, bool groupsOnly = true) {
            string result = ""; // result

            int h, d, dU, u, g = 0, e, shift;

            while(value != 0) {
                h = (int)value % 1000 / 100;
                d = (int)value % 100 / 10;
                u = (int)value % 10;

                if(h == 0 && d == 0 && u == 0) {
                    shift = g;
                    g = 0;
                } else {
                    shift = 0;
                }

                if(d == 1 && u != 0) {
                    dU = u;
                    d = 0;
                    u = 0;
                } else {
                    dU = 0;
                }

                if(u == 1 && h == 0 && d == 0 && g > 0) {
                    e = 0;
                    if(groupsOnly)
                        u = 0;
                } else if(u > 1 && u < 5) {
                    e = 1;
                } else {
                    e = 2;
                }

                result = (h == 0 ? "" : hundreds[h - 1] + " ") +
                    (d == 0 ? "" : dozens[d - 1] + " ") +
                    (dU == 0 ? "" : dozensUnities[dU - 1] + " ") +
                    (u == 0 ? "" : unities[u - 1] + " ") +
                    (g == 0 ? "" : groups[g - 1, e] + " ") +
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
        public static string translate(decimal value, Currency currency = Currency.None, bool groupsOnly = true) {
            string totalCurrency = "", decimalCurrency = "";
            bool showSign = false;

            string[] values = value.ToString().Split('.');

            long totalPart = long.Parse(values[0]);     // total part of number
            long decimalPart = 0L;                      // decimal part of number
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
                    totalCurrency = totalCurrencies[(int)currency - 1, 0];
                } else if(totalPart > 1 && totalPart < 5) {
                    totalCurrency = totalCurrencies[(int)currency - 1, 1];
                } else {
                    totalCurrency = totalCurrencies[(int)currency - 1, 2];
                }

                if(decimalPart == 1) {
                    decimalCurrency = decimalCurrencies[(int)currency - 1, 0];
                } else if(decimalPart > 1 && decimalPart < 5) {
                    decimalCurrency = decimalCurrencies[(int)currency - 1, 1];
                } else {
                    decimalCurrency = decimalCurrencies[(int)currency - 1, 2];
                }
            }

            if(values.Length == 2)
                return (showSign ? sign + " " : "") + parse(totalPart, groupsOnly) + totalCurrency + " " + parse(decimalPart, groupsOnly) + decimalCurrency;
            else
                return (showSign ? sign + " " : "") + parse(totalPart, groupsOnly) + totalCurrency;
        }
    }
}
