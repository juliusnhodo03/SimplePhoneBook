using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class CashDeposit : EntityBase
    {
        #region Mapped

        public int CashDepositId { get; set; }
        public int DepositTypeId { get; set; }
        public int SiteId { get; set; }
        public int? AccountId { get; set; }
        public int ProductTypeId { get; set; }
        public string Narrative { get; set; }
        [StringLength(50)]
        [Required]
        public string TransactionReference { get; set; }
        public decimal DepositedAmount { get; set; }
        public decimal? ActualAmount { get; set; }
        public decimal? DiscrepancyAmount { get; set; }
		public decimal? VaultAmount { get; set; } 
        public bool? HasDescripancy { get; set; }
        public bool? IsProcessed { get; set; }
        public bool? IsConfirmed { get; set; }
		public int? ProcessedById { get; set; }
		public bool? IsSubmitted { get; set; }
        public bool? IsSettled { get; set; }
        public int? DeviceId { get; set; }
        public int? SupervisorId { get; set; }
        public int StatusId { get; set; }
        public int? ErrorCodeId { get; set; }
        public string iTramsUserName { get; set; }
        public string VaultSource { get; set; }
        public string SettlementIdentifier { get; set; }
        public DateTime? CitDateTime { get; set; }
		public DateTime? ProcessedDateTime { get; set; }
        public DateTime? SettledDateTime { get; set; }
        public DateTime? SendDateTime { get; set; }
        public DateTime? SubmitDateTime { get; set; }
        public virtual DepositType DepositType { get; set; }
        public virtual Device Device { get; set; }
        public virtual Site Site { get; set; }
        public virtual Status Status { get; set; }
        public virtual Account Account { get; set; }
        public virtual ErrorCode ErrorCode { get; set; }
        public virtual ProductType ProductType { get; set; }
        public virtual Collection<Container> Containers { get; set; }
        public virtual Collection<VaultBeneficiary> VaultBeneficiaries { get; set; } 

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return CashDepositId; }
        }

		public string DepositTypeName
		{
			get { return DepositType != null ? DepositType.Name : string.Empty; }
		}
		public string ContractNumber
		{
			get { return Site != null ? Site.Merchant.ContractNumber : string.Empty; }
		}


	    public string CaptureDateString
	    {
			get { return CreateDate.Value.ToShortDateString(); }
	    }

        public string DeviceName
        {
            get { return Device != null ? Device.Name : string.Empty; }
        }

        public string SiteName
        {
            get { return Site != null ? Site.Name : string.Empty; }
        }

        public string StatusName
        {
            get { return Status != null ? Status.Name : string.Empty; }
        }

        public string AccountDetails
        {
            get { return Account != null ? Account.Bank.Name + " : " + Account.AccountNumber : string.Empty; }
        }

        public string ErrorDescription
        {
            get { return ErrorCode != null ? ErrorCode.Description : string.Empty; }
        }

        public string ProductTypeName
        {
            get { return ProductType != null ? ProductType.Name : string.Empty; }
        }

		public int BankId
		{
			get { return Account != null ? Account.BankId : 0; }
		}

        #endregion
    }
}