using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspShop.Helpers.Generators
{
    public static class RandomStringGenerator
    {
        private static Random random = new Random();
        private static string upperLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static string lowerLetters = "abcdefghijklmnopqrstuvwxyz";
        private static string numbers = "0123456789";

        public static string FullString(int length)
        {
            return String(upperLetters + numbers, length);
        }
        public static string GenerateItemCode()
        {
            return string.Format("{0}-{1}-{2}{3}", String(numbers, 2), String(numbers, 4), String(upperLetters, 2), String(numbers, 2));
        }
        public static string GenerateCustomerCode()
        {
            return string.Format("{0}-{1}", String(numbers, 4), DateTime.Now.Year);
        }
        public static string GenerateFieldId(string fieldName, int idLenght)
        {
            return string.Format("{0}{1}", fieldName, String(upperLetters + numbers, idLenght));
        }
        private static string String(string chars, int length)
        {
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}