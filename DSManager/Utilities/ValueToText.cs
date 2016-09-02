using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSManager.Utilities {
    public static class ValueToText {
        public static string translate(long value, bool groupsOnly = true) {
            string text = "";

            string[] unities = { "jeden", "dwa", "trzy", "cztery", "pięć", "sześć", "siedem", "osiem", "dziewięć" };
            string[] dozensUnities = { "jedenaście", "dwanaście", "trzynaście", "czternaście", "piętnaście", "szesnaście", "siedemnaście", "osiemnaście", "dziewiętnaście" };
            string[] dozens = {"dziesięć", "dwadzieścia", "trzydzieści", "czterdzieści", "pięćdziesiąt", "szesćdziesiąt", "siedemdziesiąt", "osiemdziesiąt", "dziewięćdziesiąt" };
            string[] hundreds = {"sto", "dwieście", "trzysta", "czterysta", "pięćset", "szesćset", "siedemset", "osiemset", "dziewięćset" };
            string[,] groups = {{"tysiąc", "tysiące", "tysięcy"},
                {"milion", "miliony", "milionów"},
                {"miliard", "miliardy", "miliardów"},
                {"bilion", "biliony", "bilionów"},
                {"biliard", "biliardy", "biliardów"},
                {"trylion", "tryliardy", "tryliardów"}
            };

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

                text = (h == 0 ? "" : hundreds[h - 1] + " ") +
                    (d == 0 ? "" : dozens[d - 1] + " ") +
                    (dU == 0 ? "" : dozensUnities[dU - 1] + " ") +
                    (u == 0 ? "" : unities[u - 1] + " ") +
                    (g == 0 ? "" : groups[g - 1, e] + " ") +
                    text;

                g += 1 + shift;
                value /= 1000;
            }

            return text;
        }
    }
}
