using System;

using DSManager.Utilities;

namespace DSManager.Validators {
    public static class BirthDateValidator {
        /// <summary>
        /// Checks if given birth date is correct with date coded in PESEL number
        /// </summary>
        /// <param name="birthDate">birth date</param>
        /// <param name="pesel">PESEL number</param>
        /// <returns>true if birthDate is equal to birth date coded in PESEL, false otherwise</returns>
        public static bool Validate(DateTime birthDate, string pesel) {
            return PESELValidator.Validate(pesel) && birthDate.Equals(PESELToDate.Translate(pesel));
        }
    }
}
