using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DSManager.Validators;

namespace DSManager.Utilities {
    public static class PESELToDate {
        public static DateTime translate(string pesel) {
            if(!PESELValidator.validate(pesel))
                return new DateTime();

            int century = 0;

            int processedNumber = int.Parse(pesel.Substring(2, 1));

            while(processedNumber != 0 && processedNumber != 1) {
                processedNumber -= 2;
                if(century == 300)
                    century = -100;
                else
                    century += 100;
            }

            int year = 1900 + int.Parse(pesel.Substring(0, 2)) + century;
            int month = int.Parse(processedNumber + pesel.Substring(3, 1));
            int day = int.Parse(pesel.Substring(4, 2));

            return new DateTime(year, month, day);
        }
    }
}
