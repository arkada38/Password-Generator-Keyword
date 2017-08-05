using System;
using System.Text;
using static System.Math;

namespace Models
{
    //http://www.passwordmeter.com/
    public static class Generator
    {
        private const string UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string LowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
        private const string Numbers = "1234567890";
        private const string SpecialCharacters = "!#$%'()*+-./:?@[]^_`{}~";

        private static long _seed;
        private const int M = (38 * 4 + 3) * (62 * 4 + 3); // p % 4 = 3 // q % 4 = 3 // m = p * q

        public static string GeneratePassword(
            string serviceName, string keyword, int passwordLength, bool useSpecialCharacters)
        {
            var serviceNameScore = GetScoreOfUtf8BytesFromString(serviceName);
            var keywordScore = GetScoreOfUtf8BytesFromString(keyword);

            _seed = serviceNameScore + serviceNameScore % 9 +
                keywordScore - keywordScore % 4 -
                passwordLength + passwordLength % 2;

            //Password should be contain uppercase, lowercase, numbers and special characters
            //without repeated characters
            //and consecutive of letters or numbers
            var quantityOfSpecialCharacters = 0;
            if (useSpecialCharacters)
                quantityOfSpecialCharacters = Max(2, passwordLength / 5);
            var quantityOfNumbers = Min(Numbers.Length, passwordLength / 4);
            var quantityOfUppercaseLetters = (passwordLength - quantityOfNumbers - quantityOfSpecialCharacters) / 2;
            var quantityOfLowercaseLetters = passwordLength - quantityOfNumbers - quantityOfSpecialCharacters - quantityOfUppercaseLetters;

            var passwordOfNumbers = string.Empty;
            var passwordOfSpecialCharacters = string.Empty;
            var passwordOfUppercaseLetters = string.Empty;
            var passwordOfLowercaseLetters = string.Empty;

            //quantityOfSpecialCharacters <= quantityOfNumbers <= quantityOfUppercaseLetters <= quantityOfLowercaseLetters

            #region Составление символов для пароля
            //uppercase
            var uppercaseLetters = UppercaseLetters;
            for (var i = 0; i < quantityOfUppercaseLetters; i++)
            {
                var j = NextRandom(uppercaseLetters.Length);
                passwordOfUppercaseLetters += uppercaseLetters[j];
                uppercaseLetters = uppercaseLetters.Remove(j, 1);
            }

            _seed += serviceName.Length;

            //lowercase
            var lowercaseLetters = LowercaseLetters;
            for (var i = 0; i < quantityOfLowercaseLetters; i++)
            {
                var j = NextRandom(lowercaseLetters.Length);
                passwordOfLowercaseLetters += lowercaseLetters[j];
                lowercaseLetters = lowercaseLetters.Remove(j, 1);
            }

            _seed += keyword.Length;

            //numbers
            var numbers = Numbers;
            for (var i = 0; i < quantityOfNumbers; i++)
            {
                var j = NextRandom(numbers.Length);
                passwordOfNumbers += numbers[j];
                numbers = numbers.Remove(j, 1);
            }

            _seed += passwordLength;

            //specialCharacters
            var specialCharacters = SpecialCharacters;
            for (var i = 0; i < quantityOfSpecialCharacters; i++)
            {
                var j = NextRandom(specialCharacters.Length);
                passwordOfSpecialCharacters += specialCharacters[j];
                specialCharacters = specialCharacters.Remove(j, 1);
            }

            _seed += serviceName.Length + keyword.Length + passwordLength;
            #endregion

            #region Перемешивание символов для пароля и его создание
            //Creation a password
            var password = string.Empty;
            var lastSymbolForPasswordFrom = string.Empty;

            for (var i = 0; i < passwordLength; i++)
            {
                //Максимальное значение символов в категории последовательности для пароля
                var maxQuantity = Max(Max(passwordOfNumbers.Length, passwordOfSpecialCharacters.Length),
                    Max(passwordOfUppercaseLetters.Length, passwordOfLowercaseLetters.Length));

                //Новый символ пароля будет из одной из самых длинных последовательностей
                var newSymbol = string.Empty;

                //Отбираем претендентов
                if (passwordOfNumbers.Length == maxQuantity && passwordOfNumbers != lastSymbolForPasswordFrom && password.Length > 0)
                    newSymbol += passwordOfNumbers[0];

                if (passwordOfSpecialCharacters.Length == maxQuantity && passwordOfSpecialCharacters != lastSymbolForPasswordFrom && password.Length > 0)
                    newSymbol += passwordOfSpecialCharacters[0];

                if (passwordOfUppercaseLetters.Length == maxQuantity && passwordOfUppercaseLetters != lastSymbolForPasswordFrom)
                    newSymbol += passwordOfUppercaseLetters[0];

                if (passwordOfLowercaseLetters.Length == maxQuantity && passwordOfLowercaseLetters != lastSymbolForPasswordFrom)
                    newSymbol += passwordOfLowercaseLetters[0];

                //Вот этот символ
                newSymbol = newSymbol[NextRandom(newSymbol.Length)].ToString();
                _seed += 1;

                //Удаляем символ из последовательности и запоминаем последовательность
                if (passwordOfNumbers.Contains(newSymbol))
                {
                    passwordOfNumbers = passwordOfNumbers.Replace(newSymbol, string.Empty);
                    lastSymbolForPasswordFrom = passwordOfNumbers;
                }
                else if (passwordOfSpecialCharacters.Contains(newSymbol))
                {
                    passwordOfSpecialCharacters = passwordOfSpecialCharacters.Replace(newSymbol, string.Empty);
                    lastSymbolForPasswordFrom = passwordOfSpecialCharacters;
                }
                else if (passwordOfUppercaseLetters.Contains(newSymbol))
                {
                    passwordOfUppercaseLetters = passwordOfUppercaseLetters.Replace(newSymbol, string.Empty);
                    lastSymbolForPasswordFrom = passwordOfUppercaseLetters;
                }
                else if (passwordOfLowercaseLetters.Contains(newSymbol))
                {
                    passwordOfLowercaseLetters = passwordOfLowercaseLetters.Replace(newSymbol, string.Empty);
                    lastSymbolForPasswordFrom = passwordOfLowercaseLetters;
                }

                //И добавляем к паролю
                password += newSymbol;
            }
            #endregion

            return password;
        }

        public static int GetScoreOfUtf8BytesFromString(string s)
        {
            var score = 0;
            var utf8Bytes = Encoding.UTF8.GetBytes(s);

            for (var i = 0; i < utf8Bytes.Length; i++)
            {
                score += utf8Bytes[i];
                if (i % 2 != 0)
                    score += utf8Bytes[i];
                else
                    score -= utf8Bytes[i] * 2;
            }

            while (score > 1000)
                score -= 1000;

            return score;
        }

        private static int NextRandom(int n)
        {
            _seed = _seed * _seed % M;
            var x = (n - 1) * (double)_seed / M;
            return Convert.ToInt32(Round(x, MidpointRounding.AwayFromZero));
        }
    }
}
