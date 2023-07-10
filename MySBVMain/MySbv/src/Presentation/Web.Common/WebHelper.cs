using System.Collections.Generic;
using System.Linq;
using Domain.Data.Core;
using Domain.Data.Model;

namespace Web.Common
{
    public static class WebHelper
    {
        /// <summary>
        ///     ToDropDown Model
        /// </summary>
        /// <typeparam name="T">Generic Type argument</typeparam>
        /// <param name="collection">Collection</param>
        /// <returns></returns>
        public static IEnumerable<DropDownModel> ToDropDownModel<T>(this IEnumerable<T> collection)
            where T : IEntity, IIdentity
        {
            List<DropDownModel> selection = collection.Select(item => new DropDownModel
            {
                Id = item.Key,
                Name = item.Name,
                Text = item.Description,
                Tag = item.LookUpKey
            }).ToList();
            selection.Insert(0, new DropDownModel {Id = 0, Name = "Please Select", Text = "Please Select", Tag = ""});
            return selection;
        }

        /// <summary>
        ///     Remove First
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IEnumerable<DropDownModel> RemoveFirst(this IEnumerable<DropDownModel> collection)
        {
            return collection.Skip(1);
        }

        /// <summary>
        ///     ToDenomination DropDownModel
        /// </summary>
        /// <param name="collection">Collection</param>
        /// <returns></returns>
        public static IEnumerable<DropDownModel> ToDenominationDropDownModel(this IEnumerable<Denomination> collection)
        {
            List<DropDownModel> selectionOptions = collection.Select(item => new DropDownModel
            {
                Id = item.ValueInCents,
                Name = (item.ValueInCents/100 < 1) ? item.Name + "c" : item.Name,
                Tag = item.ValueInCents
            }).ToList();
            selectionOptions.Insert(0, new DropDownModel {Id = 1000500, Name = "Please Select", Tag = 0});
            return selectionOptions;
        }


        /// <summary>
        ///     ToBanks DropDownModel
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IEnumerable<DropDownModel> ToBanksDropDownModel(this IEnumerable<Bank> collection)
        {
            List<DropDownModel> selectionOptions = collection.Select(item => new DropDownModel
            {
                Id = item.BankId,
                Name = item.Name,
                Text = item.BranchCode
            }).ToList();
            selectionOptions.Insert(0, new DropDownModel {Id = 1000500, Name = "Please Select", Tag = 0});
            return selectionOptions;
        }

        /// <summary>
        ///     ToDepositReference DropDownModel
        /// </summary>
        /// <param name="collection">Collection</param>
        /// <returns></returns>
        public static IEnumerable<DropDownModel> ToDepositReferenceDropDownModel(this Dictionary<int, string> collection)
        {
            List<DropDownModel> selectionOptions = collection.Select(item => new DropDownModel
            {
                Id = item.Key,
                Name = item.Value
            }).ToList();
            return selectionOptions;
        }


        /// <summary>
        ///     ToAccounts DropDownModel
        /// </summary>
        /// <param name="collection">Collection</param>
        /// <returns></returns>
        public static IEnumerable<DropDownModel> ToAccountsDropDownModel(this IEnumerable<Account> collection)
        {
            List<DropDownModel> selectionOptions = collection.Select(item => new DropDownModel
            {
                Id = item.Key,
                Name = item.AccountNumber
            }).ToList();
            selectionOptions.Insert(0, new DropDownModel {Id = 0, Name = "Please Select", Tag = ""});
            return selectionOptions;
        }


        //new DropDownModel { Id = e.SiteSettlementAccountId, Name = e.AccountNumber }

        /// <summary>
        ///     ToSitesDrop DownModel
        /// </summary>
        /// <param name="collection">Collection</param>
        /// <returns></returns>
        public static IEnumerable<DropDownModel> ToSitesDropDownModel(this IEnumerable<Site> collection)
        {
            List<DropDownModel> selection = collection.Select(item => new DropDownModel
            {
                Id = item.Key,
                Name = item.Name,
                Text = item.CitCode,
                Value = item.DepositReference,
                Tag = item.CitCarrier.SerialStartNumber
            }).ToList();
            selection.Insert(0, new DropDownModel {Id = 0, Name = "Please Select", Tag = ""});
            return selection;
        }

        /// <summary>
        ///     ToFee Model
        /// </summary>
        /// <param name="collection">Collection</param>
        /// <returns></returns>
        public static IEnumerable<FeeModel> ToFeeModel(this IEnumerable<Fee> collection)
        {
            return collection.Select(item => new FeeModel
            {
                code = item.Code
            });
        }
    }

    /// <summary>
    ///     Fee Model Class
    /// </summary>
    public class FeeModel
    {
        public string code { get; set; }
    }
}