namespace DSManager.PDF.Utilities {
    public class AddLeadingZeros {
        public static string Convert(int? value) {
            return value == null ? "00" : value < 10 ? "0" + value : value.ToString();
        }
    }
}