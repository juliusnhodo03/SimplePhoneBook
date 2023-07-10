using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Modules.UserAccountValidation;
using Domain.Data.Model;
using Domain.Repository;

namespace Application.Modules.Maintanance.Users
{
    public static class UserHelper
    {
        /// <summary>
        /// Generate a new from first name and last name
        /// </summary>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <returns></returns>
        public static string GenerateUserName(string firstname, string lastname)
        {
            string fullName = string.Empty;
            if (firstname.Length < 12)
                fullName = firstname + lastname.Substring(0, 1);
            else fullName = firstname.Substring(0, 11) + lastname.Substring(0, 1);

            while (fullName.Length < 6)
            {
                fullName += (new Random().Next(1, 10));
            }

            if (fullName.Length > 12)
                return fullName.Substring(0, 11);

            string fixedName = string.Empty;

            foreach (char t in fullName)
            {
                if (char.IsLetterOrDigit(t))
                    fixedName += t;
                if (t == '.' || t == '-' || t == '_')
                    fixedName += t;
            }
            return fixedName;
        }

        /// <summary>
        /// Generate reset a user's password by generating a random one.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="userAccountValidation"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static ResetPasswordResult ResetPassword(IRepository repository,IUserAccountValidation userAccountValidation, string username)
        {
            User userInformation = repository.Query<User>(a => a.UserName.ToLower() == username.ToLower()).FirstOrDefault();

            if (userInformation != null)
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYabcdefghijklmnopqrstuvwxyzZ0123456789";
                const string special = "#$%(){}";
                var random = new Random();
                var password = new string(Enumerable.Repeat(chars, 3).Select(s => s[random.Next(s.Length)]).ToArray());
                password += new string(Enumerable.Repeat(special, 1).Select(s => s[random.Next(s.Length)]).ToArray());
                password += new string(Enumerable.Repeat(chars, 3).Select(s => s[random.Next(s.Length)]).ToArray());

                if (userAccountValidation.ResetPassword(username, password))
                {
                    return new ResetPasswordResult
                    {
                        Result = true,
                        FullNames = userInformation.FirstName + " " + userInformation.LastName,
                        Password = password,
                        EmailAddress = userInformation.EmailAddress
                    };
                }
            }
            return new ResetPasswordResult();
        }

        public class ResetPasswordResult
        {
            public ResetPasswordResult()
            {
                Result = false;
            }

            public string FullNames { get; set; }

            public string Password { get; set; }

            public string EmailAddress { get; set; }

            public bool Result { get; set; }
        }
    }
}
