using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Application.Dto.ValidationAttributes
{
    public enum Gender
    {
        Unknown,
        Male,
        Female
    }

    public class IdentityInfo
    {
        public IdentityInfo(string identityNumber)
        {
            Initialize(identityNumber);
        }

        public string IdentityNumber { get; private set; }

        public DateTime BirthDate { get; private set; }

        public Gender Gender { get; private set; }

        public bool IsSouthAfrican { get; private set; }

        public bool IsValid { get; private set; }

        private void Initialize(string identityNumber)
        {
            IdentityNumber = (identityNumber ?? string.Empty).Replace(" ", "");
            if (IdentityNumber.Length == 13)
            {
                var digits = new int[13];
                for (int i = 0; i < 13; i++)
                {
                    digits[i] = int.Parse(IdentityNumber.Substring(i, 1));
                }
                int control1 = digits.Where((v, i) => i%2 == 0 && i < 12).Sum();
                string second = string.Empty;
                digits.Where((v, i) => i%2 != 0 && i < 12).ToList().ForEach(v =>
                    second += v.ToString());
                string string2 = (int.Parse(second)*2).ToString();
                int control2 = 0;
                for (int i = 0; i < string2.Length; i++)
                {
                    control2 += int.Parse(string2.Substring(i, 1));
                }
                int control = (10 - ((control1 + control2)%10))%10;
                if (digits[12] == control)
                {
                    BirthDate = DateTime.ParseExact(IdentityNumber
                        .Substring(0, 6), "yyMMdd", null);
                    Gender = digits[6] < 5 ? Gender.Female : Gender.Male;
                    IsSouthAfrican = digits[10] == 0;
                    IsValid = true;
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class IsValidIdAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return string.IsNullOrEmpty((string) value) || new IdentityInfo((string) value).IsSouthAfrican;
        }
    }
}