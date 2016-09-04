using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSManager.Validators {
    public static class PESELValidator {
        /// <summary>
        /// Checks if given PESEL is valid
        /// </summary>
        /// <param name="pesel">PESEL number</param>
        /// <returns>true if PESEL is valid, false otherwise</returns>
        public static bool validate(string pesel) {
            if(pesel == null || pesel.Length != 11)
                return false;
            else {
                int monthPart = int.Parse(pesel.Substring(2, 2));
                if((monthPart > 12 && monthPart < 21) ||
                    (monthPart > 32 && monthPart < 41) ||
                    (monthPart > 52 && monthPart < 61) ||
                    (monthPart > 72 && monthPart < 81) ||
                    int.Parse(pesel.Substring(2, 2)) > 92)
                        return false;
            }

            int processedNumber, controlNumber = 0, multiplier = 1;

            for(int i = 0; i <= pesel.Length - 2; i++) {
                processedNumber = int.Parse(pesel.Substring(i, 1));
                controlNumber += (multiplier * processedNumber) % 10;

                if(multiplier == 3)
                    multiplier = 7;
                else if(multiplier == 9)
                    multiplier = 1;
                else
                    multiplier += 2;
            }

            controlNumber = (10 - (controlNumber % 10)) % 10;
            processedNumber = int.Parse(pesel.Substring(10, 1));

            if(processedNumber == controlNumber)
                return true;

            return false;
        }
    }
}