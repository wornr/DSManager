using System;

using DSManager.Validators;

namespace DSManager.Utilities {
    // ReSharper disable once InconsistentNaming
    public static class PESELToDate {
        public static DateTime Translate(string pesel) {
            if(!PESELValidator.Validate(pesel))
                return new DateTime();

            var century = 0;

            var processedNumber = int.Parse(pesel.Substring(2, 1));

            while(processedNumber != 0 && processedNumber != 1) {
                processedNumber -= 2;
                if(century == 300)
                    century = -100;
                else
                    century += 100;
            }

            var year = 1900 + int.Parse(pesel.Substring(0, 2)) + century;
            var month = int.Parse(processedNumber + pesel.Substring(3, 1));
            var day = int.Parse(pesel.Substring(4, 2));

            return new DateTime(year, month, day);
        }
    }
}
