using System;

namespace Application.Dto.CashOrder
{
    public class CashOrderMerchantDto
    {
        public int MerchantId { get; set; }
        public int? MerchantDescriptionId { get; set; }
        public int CompanyTypeId { get; set; }
        public int? StatusId { get; set; }
        public string MerchantName { get; set; }
        public string MerchantNumber { get; set; }
        public string Description { get; set; }
        public string TradingName { get; set; }
        public string CompanyGroupName { get; set; }
        public string FranchiseName { get; set; }
        public string RegisteredName { get; set; }
        public string RegistrationNumber { get; set; }
        public string VATNumber { get; set; }
        public string ContractNumber { get; set; }
        public DateTime? ImplementationDate { get; set; }
        public DateTime? TerminationDate { get; set; }
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

        public bool IsApproved { get; set; }

        public DateTime CapturedTimestamp { get; set; }
        public string Comments { get; set; }
    }
}