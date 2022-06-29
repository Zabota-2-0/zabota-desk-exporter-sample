using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clientix.Helpers
{
    static class StringExtensions
    {
        public static string WordByIndex(this string str, int index, params char[] separator)
        {
            if (str == null)
            {
                return "";
            }

            string[] words = str.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            if (index < words.Length)
            {
                return words[index];
            }

            return "";           
          
        }
    }
}
