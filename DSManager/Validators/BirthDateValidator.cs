using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DSManager.Utilities;

namespace DSManager.Validators {
    public static class BirthDateValidator {
        public static bool validate(DateTime birthDate, string pesel) {
            if(birthDate == null || !PESELValidator.validate(pesel))
                return false;

            if(birthDate.Equals(PESELToDate.translate(pesel)))
                return true;

            return false;
        }
    }
}
