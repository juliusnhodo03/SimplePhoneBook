using System;
using System.Collections.ObjectModel;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class Site : EntityBase, IIdentity
    {
        #region Mapped

        public int SiteId { get; set; }
        public int MerchantId { get; set; }
        public int CitCarrierId { get; set; }
        public int StatusId { get; set; }
        public int CityId { get; set; }
        public int CashCenterId { get; set; } 
        public int AddressId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LookUpKey { get; set; }
        public string CitCode { get; set; }
        public string SysproNumber { get; set; }
        public string PostalAddress { get; set; }
        public string DepositReference { get; set; }
        public bool DepositReferenceIsEditable { get; set; }
        public string ContactPersonName1 { get; set; }
        public string ContactPersonEmailAddress1 { get; set; }
        public string ContactPersonNumber1 { get; set; }
        public string ContactPersonDesignation1 { get; set; }
        public string ContactPersonName2 { get; set; }
        public string ContactPersonEmailAddress2 { get; set; }
        public string ContactPersonNumber2 { get; set; }
        public string ContactPersonDesignation2 { get; set; }
        public bool ApprovalRequiredFlag { get; set; }
		public bool IsCashCentreAllowedDepositCapturing { get; set; }  
        public string Comments { get; set; }
        public DateTime? ImplementationDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public bool IsActive { get; set; }
        public virtual Merchant Merchant { get; set; }
        public virtual Status Status { get; set; }
        public virtual City City { get; set; }
        public virtual CitCarrier CitCarrier { get; set; }
        public virtual CashCenter CashCenter { get; set; }
        public virtual Address Address { get; set; }
        public virtual Collection<CashDeposit> CashDeposits { get; set; }
        public virtual Collection<Account> Accounts { get; set; }
        public virtual Collection<SiteContainer> SiteContainers { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return SiteId; }
        }

        public string MerchantName
        {
            get { return Merchant != null ? Merchant.Name : string.Empty; }
        }

        public string ContractNumber
        {
            get { return Merchant != null ? Merchant.ContractNumber : string.Empty; }
        }

        public string CityName
        {
            get { return City != null ? City.Name : string.Empty; }
        }

        public string CitCarrierName
        {
            get { return CitCarrier != null ? CitCarrier.Name : string.Empty; }
        }

        public string CashCenterName
        {
            get { return CashCenter != null ? CashCenter.Name : string.Empty; }
        }

        
        #endregion
    }
}