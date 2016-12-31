using System.Text;

using System.Security.Cryptography;

namespace DSManager.Utilities {
    // ReSharper disable once InconsistentNaming
    public static class MD5Encrypter {
        public static string Encrypt(string text) {
            MD5 md5 = new MD5CryptoServiceProvider();

            md5.ComputeHash(Encoding.ASCII.GetBytes(text));

            var result = md5.Hash;

            var strBuilder = new StringBuilder();
            foreach (var t in result) {
                strBuilder.Append(t.ToString("x2"));
            }

            return strBuilder.ToString();
        }
    }
}
