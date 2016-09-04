using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DSManager.Utilities;

namespace DSManager.Validators {
    public static class BirthDateValidator {
        /// <summary>
        /// Checks if given birth date is correct with date coded in PESEL number
        /// </summary>
        /// <param name="birthDate">birth date</param>
        /// <param name="pesel">PESEL number</param>
        /// <returns>true if birthDate is equal to birth date coded in PESEL, false otherwise</returns>
        public static bool validate(DateTime birthDate, string pesel) {
            if(birthDate == null || !PESELValidator.validate(pesel))
                return false;

            if(birthDate.Equals(PESELToDate.translate(pesel)))
                return true;

            return false;
        }
    }
}
