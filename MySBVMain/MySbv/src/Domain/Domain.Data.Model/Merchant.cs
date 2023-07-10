using System;
using System.Collections.ObjectModel;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class Merchant : EntityBase, IIdentity
    {
        #region Mapped

        public Merchant()
        {
            Sites = new Collection<Site>();
        }

        public int MerchantId { get; set; }
        public int? MerchantDescriptionId { get; set; }
        public int CompanyTypeId { get; set; }
        public int? StatusId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LookUpKey { get; set; }
        public string Number { get; set; }
        public string TradingName { get; set; }
        public string CompanyGroupName { get; set; }
        public string FranchiseName { get; set; }
        public string RegisteredName { get; set; }
        public string RegistrationNumber { get; set; }
// ReSharper disable once InconsistentNaming
        public string VATNumber { get; set; }
        public string ContractNumber { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactPerson1 { get; set; }
        public string ContactPerson2 { get; set; }
        public string ContactPerson1Phone { get; set; }
        public string ContactPerson2Phone { get; set; }
        public string ContactPerson1Designation { get; set; }
        public string ContactPerson2Designation { get; set; }
        public string ContactPerson1EmailAddress { get; set; }
        public string ContactPerson2EmailAddress { get; set; }
        public string ContractDocumentUrl { get; set; }
        public string WebSiteUrl { get; set; }
        public string FinancialAccountant { get; set; }
        public string FinancialAccountantEmailAddress { get; set; }
        public bool DepositSlipEmailIndicator { get; set; }
        public string Comments { get; set; }
        public DateTime? ImplementationDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public virtual CompanyType CompanyType { get; set; }
        public virtual MerchantDescription MerchantDescription { get; set; }
        public virtual Status Status { get; set; }
        public virtual Collection<Site> Sites { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return MerchantId; }
        }

        public string CompanyTypeName
        {
            get { return CompanyType != null ? CompanyType.Name : string.Empty; }
        }

        public string MerchantDescriptionName
        {
            get { return MerchantDescription != null ? MerchantDescription.Name : string.Empty; }
        }

        public string StatusName
        {
            get { return Status != null ? Status.Name : string.Empty; }
        }

        #endregion
    }
}