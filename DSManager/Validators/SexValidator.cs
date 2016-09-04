using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DSManager.Model.Enums;

namespace DSManager.Validators {
    public static class SexValidator {
        public static bool validate(Sex sex, string pesel) {
            if(sex == Sex.None || !PESELValidator.validate(pesel))
                return false;

            int sexNumber = int.Parse(pesel.Substring(9, 1));

            if((sexNumber % 2 == 0 && sex == Sex.Female) || (sexNumber % 2 == 1 && sex == Sex.Male))
                return true;

            return false;
        }
    }
}
