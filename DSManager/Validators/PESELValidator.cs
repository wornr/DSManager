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

            var monthPart = int.Parse(pesel.Substring(2, 2));
            if((monthPart > 12 && monthPart < 21) ||
               (monthPart > 32 && monthPart < 41) ||
               (monthPart > 52 && monthPart < 61) ||
               (monthPart > 72 && monthPart < 81) ||
               int.Parse(pesel.Substring(2, 2)) > 92)
                return false;

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