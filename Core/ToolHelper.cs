using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
 
namespace Core
{

    public class ToolHelper
    {
        public enum Result // Sonuç statüleri (Başarılı , Başarısız vb..)
        {
            Fail = 0, // Başarısız
            Success = 1 // Başarılı
        }

        public static bool IsNumber(object value) // Rakam/Sayı/Numara mı ?
        {
            bool isNum;
            long retNum;

            isNum = long.TryParse(Convert.ToString(value), System.Globalization.NumberStyles.Integer, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        public static bool IsStringNullOrWhiteSpace(string value) // String "     " veya null kontrolü 
        {
            if (!string.IsNullOrWhiteSpace(value))
                return false;

            return true;
        }

        public static bool IsStringNullorEmpty(string value) // String "" veya null kontrolü
        {
            if (!string.IsNullOrEmpty(value))
                return false;

            return true;
        }

        public static int? ConvertToInt32(string value) // Int32'ye çevir
        {
            int number;
            bool result = Int32.TryParse(value, out number);

            if (result)
                return number;

            return null;
        }

        public static bool ConvertToBool(string value) // Bool'a çevir. "True","False","1","0" için çalışıyor bunların dışındakilere tümünü false olarak return ediyor (Boşluk vs gibi)
        {
            var boolValue = false;
            if (bool.TryParse(value, out boolValue))
                return boolValue;

            var number = 0;
            int.TryParse(value, out number);
            return Convert.ToBoolean(number);
        }

        public static long? ConvertToLong(string value) // Long'a çevir
        {
            long number;
            bool result = long.TryParse(value, out number);

            if (result)
                return number;

            return null;
        }

        public static DateTime? ConvertToDateTime(string value) //Datetime'a çevir
        {
            DateTime dateValue;
            CultureInfo culture = CultureInfo.CurrentCulture;
            DateTimeStyles styles = DateTimeStyles.None;

            if (DateTime.TryParse(value, culture, styles, out dateValue))
                return dateValue;

            return null;
        }

        public static List<string> SplitByComma(string value) // Virgül (",")'e göre ayır ve string liste olarak döndür
        {
            return new List<string>(value.Split(','));
        }

        public static List<int> SplitByCommaConvertToInt32(string value) // Virgül (",")'e göre ayır ve int liste olarak döndür (String içerisinde 1,2,3,4 gibi rakamlar için çalışır)
        {
            return new List<int>(value.Split(',').Select(int.Parse));
        }

        public static string TurkishCharacterReplace(string Text) // Türkçe karakteri İngilizce karaktere çevir
        {
            return Text.Replace("ı", "i").Replace("İ", "I").
                        Replace("â", "a").
                        Replace("ç", "c").Replace("Ç", "C").
                        Replace("ğ", "g").Replace("Ğ", "G").
                        Replace("ö", "o").Replace("Ö", "O").
                        Replace("ş", "s").Replace("Ş", "S").
                        Replace("ü", "u").Replace("Ü", "U");
        }

        private static string key = "1b48f5effrhreherh43353hrthrhrthrthrthrthkukk..86jrww5asdasdjh5hj3gb5ad20db7834acf"; // _Encrypt ve _Decrypt metotları için oluşturduk

        public static string _Encrypt(string value) // String Şifrele
        {
            Byte[] inputArray = UTF8Encoding.UTF8.GetBytes(value);
            TripleDESCryptoServiceProvider TripleDes = new TripleDESCryptoServiceProvider();
            TripleDes.Key = UTF8Encoding.UTF8.GetBytes(key);
            TripleDes.Mode = CipherMode.ECB;
            TripleDes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = TripleDes.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            TripleDes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string _Decrypt(string value) // String Şifre Çöz
        {
            Byte[] inputArray = Convert.FromBase64String(value);
            TripleDESCryptoServiceProvider TripleDes = new TripleDESCryptoServiceProvider();
            TripleDes.Key = UTF8Encoding.UTF8.GetBytes(key);
            TripleDes.Mode = CipherMode.ECB;
            TripleDes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = TripleDes.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            TripleDes.Clear();

            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public static bool EmailAddressCheck(string emailAddress) // String Email mi Kontrolü
        {
            bool returnValue = false;

            string pattern = "^[a-zA-Z][\\w\\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\\w\\.-]*[a-zA-Z0-9]\\.[a-zA-Z][a-zA-Z\\.]*[a-zA-Z]$";
            Match emailAddressMatch = Regex.Match(emailAddress, pattern);

            if (emailAddressMatch.Success)
                returnValue = true;

            return returnValue;
        }

        public static int getMonthDays(int year, int month) // Bugüne göre Ay içindeki gün sayısını bulur (Şubat 28 gün gibi)
        {
            return DateTime.DaysInMonth(year, month);
        }

        public static bool IsUrl(string url) // string Url mi değil mi kontrolü
        {
            Uri outUri;
            if (Uri.TryCreate(url, UriKind.Absolute, out outUri) && (outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps)) // Url doğru ise devam et
                return true;

            return false;
        }

        public static string Md5(string text)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(text);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }

            return sb.ToString();
        }
 
 
 
    }
}
