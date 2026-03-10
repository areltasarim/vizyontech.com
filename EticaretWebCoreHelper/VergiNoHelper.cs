using System;
using System.Linq;

namespace EticaretWebCoreHelper
{
    public static class VergiNoHelper
    {
        /// <summary>
        /// TC Kimlik Numarası doğrulama
        /// </summary>
        public static bool TcKimlikNoDogrula(string tcKimlikNo)
        {
            if (string.IsNullOrWhiteSpace(tcKimlikNo))
                return false;

            // 11 haneli olmalı
            if (tcKimlikNo.Length != 11)
                return false;

            // Sadece rakam içermeli
            if (!tcKimlikNo.All(char.IsDigit))
                return false;

            // İlk hane 0 olamaz
            if (tcKimlikNo[0] == '0')
                return false;

            int[] digits = tcKimlikNo.Select(c => int.Parse(c.ToString())).ToArray();

            // 1, 3, 5, 7, 9. hanelerin toplamının 7 katından
            // 2, 4, 6, 8. hanelerin toplamı çıkarıldığında elde edilen sonucun
            // 10'a bölümünden kalan, 10. haneyi vermelidir
            int oddSum = digits[0] + digits[2] + digits[4] + digits[6] + digits[8];
            int evenSum = digits[1] + digits[3] + digits[5] + digits[7];
            int digit10 = ((oddSum * 7) - evenSum) % 10;

            if (digit10 != digits[9])
                return false;

            // İlk 10 hanenin toplamının 10'a bölümünden kalan, 11. haneyi vermelidir
            int sum = digits.Take(10).Sum();
            int digit11 = sum % 10;

            return digit11 == digits[10];
        }

        /// <summary>
        /// Vergi Numarası doğrulama (10 haneli)
        /// </summary>
        public static bool VergiNumarasiDogrula(string vergiNo)
        {
            if (string.IsNullOrEmpty(vergiNo) || vergiNo.Length != 10 || !vergiNo.All(char.IsDigit))
                return false;

            int[] digits = vergiNo.Select(x => int.Parse(x.ToString())).ToArray();
            int sum = 0;
            
            for (int i = 0; i < 9; i++)
            {
                int tmp = (digits[i] + (9 - i)) % 10;
                int pow = (int)Math.Pow(2, 9 - i) % 9;
                int result = tmp * pow;
                
                if (tmp != 0 && result % 9 == 0)
                    result = 9;
                else
                    result %= 9;
                    
                sum += result;
            }
            
            int check = (10 - (sum % 10)) % 10;
            return digits[9] == check;
        }

        /// <summary>
        /// Vergi numarası veya TC kimlik numarası doğrulama
        /// </summary>
        public static (bool IsValid, string Message) VergiNoVeyaTcKimlikDogrula(string numara)
        {
            if (string.IsNullOrWhiteSpace(numara))
                return (false, "Vergi numarası veya TC kimlik numarası boş olamaz");

            // Sadece rakam kontrolü
            if (!numara.All(char.IsDigit))
                return (false, "Sadece rakam girebilirsiniz");

            // 10 haneli ise vergi numarası kontrolü
            if (numara.Length == 10)
            {
                if (VergiNumarasiDogrula(numara))
                    return (true, "Vergi numarası geçerli");
                else
                    return (false, "Geçersiz vergi numarası");
            }
            // 11 haneli ise TC kimlik numarası kontrolü
            else if (numara.Length == 11)
            {
                if (TcKimlikNoDogrula(numara))
                    return (true, "TC kimlik numarası geçerli");
                else
                    return (false, "Geçersiz TC kimlik numarası");
            }
            else
            {
                return (false, "Vergi numarası 10 haneli veya TC kimlik numarası 11 haneli olmalıdır");
            }
        }
    }
}
