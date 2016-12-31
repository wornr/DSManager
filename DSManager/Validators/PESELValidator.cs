namespace DSManager.Validators {
    // ReSharper disable once InconsistentNaming
    public static class PESELValidator {
        /// <summary>
        /// Checks if given PESEL is valid
        /// </summary>
        /// <param name="pesel">PESEL number</param>
        /// <returns>true if PESEL is valid, false otherwise</returns>
        public static bool Validate(string pesel) {
           if(pesel == null || pesel.Length != 11)
                return false;

            var monthPart = int.Parse(pesel.Substring(2, 2)) % 20;
            if(monthPart < 1 || monthPart > 12)
                return false;

            var dayPart = int.Parse(pesel.Substring(4, 2));
            switch (monthPart) {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    if (dayPart > 31)
                        return false;
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    if(dayPart > 30)
                        return false;
                    break;
                case 2:
                    if (dayPart > 29)
                        return false;
                    break;
            }

            int processedNumber, controlNumber = 0, multiplier = 1;

            for(var i = 0; i <= pesel.Length - 2; i++) {
                processedNumber = int.Parse(pesel.Substring(i, 1));
                controlNumber += (multiplier * processedNumber) % 10;

                switch (multiplier) {
                    case 3:
                        multiplier = 7;
                        break;
                    case 9:
                        multiplier = 1;
                        break;
                    default:
                        multiplier += 2;
                        break;
                }
            }

            controlNumber = (10 - (controlNumber % 10)) % 10;
            processedNumber = int.Parse(pesel.Substring(10, 1));

            return processedNumber == controlNumber;
        }
    }
}