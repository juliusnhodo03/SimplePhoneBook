using System;
using System.Text;

namespace SimplePhoneBook.API.Extensions
{
    public static class CasingExtentions
    {
        /// <summary>
        ///     Capitalize a sentence.
        /// </summary>
        /// <param name="stringInput"></param>
        public static string ToProperCase(this string stringInput)
        {
            var sb = new StringBuilder();
            bool fEmptyBefore = true;
            foreach (char ch in stringInput)
            {
                char chThis = ch;
                if (Char.IsWhiteSpace(chThis))
                    fEmptyBefore = true;
                else
                {
                    if (Char.IsLetter(chThis) && fEmptyBefore)
                        chThis = Char.ToUpper(chThis);
                    else
                        chThis = Char.ToLower(chThis);
                    fEmptyBefore = false;
                }
                sb.Append(chThis);
            }
            return sb.ToString().Trim();
        }
    }
}

