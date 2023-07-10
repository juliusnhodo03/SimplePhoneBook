using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using Domain.Data.Core;
using Domain.Data.Model;
using Domain.Repository;

namespace Application.Modules.Common
{
    public enum Function
    {
        Add,
        Update,
        Delete
    }

    public enum DbTable
    {
        Cashdeposit,
        MultiDeposit,
        Vaultpayments
    }

    public static class ApplicationHelpers
    {
        /// <summary>
        /// Generate a task Number for approval
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        public static string GenerateTaskNumber(IRepository repository)
        {
            int count = repository.All<Task>().Count();
            string taskReference = string.Concat("MYSBV", DateTime.Now.Year.ToString(CultureInfo.InvariantCulture),
                DateTime.Now.Month.ToString(CultureInfo.InvariantCulture),
                DateTime.Now.Day.ToString(CultureInfo.InvariantCulture), count.ToString(CultureInfo.InvariantCulture));
            return taskReference;
        }

        /// <summary>
        /// Check if a username is available for use
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool UserNameExist(IRepository repository, string userName)
        {
            return repository.Any<User>(a => a.UserName.ToLower() == userName.ToLower());
        }

        /// <summary>
        /// Check if an Id Number has been used before on the system
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="userId"></param>
        /// <param name="username"></param>
        /// <param name="id"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        public static bool UserIdNumberExist(IRepository repository, string username, string id, Function function = Function.Add)
        {
            switch (function)
            {
                case Function.Add:
                    return repository.Any<User>(a => a.IdNumber == id || a.PassportNumber == id);
                case Function.Update:
                    var user = repository.Query<User>(a => a.UserName == username).FirstOrDefault();
                    return repository.Any<User>(a => a.IdNumber == id && a.UserId != user.UserId || a.PassportNumber == id && a.UserId != user.UserId);
                case Function.Delete:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException("function");
            }
        }


        /// <summary>
        /// Convert a hashset to a string
        /// </summary>
        /// <param name="hashSet"></param>
        /// <returns></returns>
		public static string ToHashString(this HashSet<string> hashSet)
		{
			return hashSet.Aggregate(string.Empty, (current, ints) => current + ints);
		}

        /// <summary>
        /// Remove deleted items from the cash deposit object graph
        /// </summary>
        /// <param name="deposit"></param>
        /// <returns></returns>
        public static CashDeposit RemoveDeleted(this CashDeposit deposit)
        {
            foreach (var container in deposit.Containers.ToList())
            {
                foreach (var containerDrop in container.ContainerDrops.ToList())
                {
                    foreach (var containerDropItem in containerDrop.ContainerDropItems.ToList())
                    {
                        if (!containerDropItem.IsNotDeleted)
                            containerDrop.ContainerDropItems.Remove(containerDropItem);
                    }

                    if (!containerDrop.IsNotDeleted)
                        container.ContainerDrops.Remove(containerDrop);
                }

                if (!container.IsNotDeleted)
                    deposit.Containers.Remove(container);
            }
            return deposit;
        }

        public static string GenerateSettlementIdentifier(DbTable table, IRepository repository)
        {
            switch (table)
            {
                case DbTable.Cashdeposit:
                    return GenerateForCashDeposit(repository);
                case DbTable.MultiDeposit:
                    return GenerateForMultiDeposit(repository);
                case DbTable.Vaultpayments:
                    return GenerateForVaultPayments(repository);
                default:
                    throw new ArgumentException("Invalid Table Type");
            }
        }

        private static string GenerateForCashDeposit(IRepository repository)
        {
            string code = string.Concat("CD", Guid.NewGuid().ToString("N").Substring(0, 10));
            while (repository.Any<VaultPartialPayment>(a => a.SettlementIdentifier == code))
            {
                code = string.Concat("CD", Guid.NewGuid().ToString("N").Substring(0, 10));
            }
            return code;
        }

        private static string GenerateForMultiDeposit(IRepository repository)
        {
            string code = string.Concat("MD", Guid.NewGuid().ToString("N").Substring(0, 10));
            while (repository.Any<VaultPartialPayment>(a => a.SettlementIdentifier == code))
            {
                code = string.Concat("MD", Guid.NewGuid().ToString("N").Substring(0, 10));
            }
            return code;
        }

        private static string GenerateForVaultPayments(IRepository repository)
        {
            string code = string.Concat("VP", Guid.NewGuid().ToString("N").Substring(0, 10));
            while (repository.Any<VaultPartialPayment>(a => a.SettlementIdentifier == code))
            {
                code = string.Concat("VP", Guid.NewGuid().ToString("N").Substring(0, 10));
            }
            return code;
        }

        public static Site RemoveDeletedInactiveAccounts(this Site site, ILookup lookup)
        {
            if (site == null) return null;

            var status = lookup.GetStatusId("ACTIVE");
            foreach (var account in site.Accounts.ToList())
            {
                if (!account.IsNotDeleted || account.StatusId != status)
                    site.Accounts.Remove(account);
            }
            return site;
        }

    }
}
