using System;
using System.ComponentModel.DataAnnotations;
using Application.Dto.Users;

namespace Application.Dto.ValidationAttributes
{
    public class IsValidUsernameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var userDto = (UserDto)validationContext.ObjectInstance;
            return VerifyBusinessRules(userDto);
        }

        private ValidationResult VerifyBusinessRules(UserDto userDto)
        {
            return userDto.UserName == GenerateUsername(userDto)
                ? ValidationResult.Success
                : new ValidationResult("Invalid Username");
        }

        private string GenerateUsername(UserDto userDto)
        {
            string fullName = string.Concat(userDto.FirstName,
                userDto.LastName.Substring(0, 1));

            int count = 1;
            while (fullName.Length <= 6)
            {
                if (userDto.LastName.Length + 1 >= count)
                    fullName += userDto.LastName.Substring(count, 1);
                else
                    fullName.PadRight(new Random().Next(100, 1000));
                count++;
            }

            if (fullName.Length > 12)
                return fullName.Substring(0, 11);

            return fullName;
        }
    }
}