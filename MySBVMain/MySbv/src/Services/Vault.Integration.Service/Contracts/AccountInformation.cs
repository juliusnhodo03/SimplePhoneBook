using System;
using System.Runtime.Serialization;

namespace Vault.Integration.Service.Contracts
{
    [DataContract(Namespace = "http://www.sbv.co.za/deposit")]
    public class AccountInformation
    {
        [DataMember]
        [Obsolete("Use Beneficiary Code instead of AccountId. AccountId will be removed in future updates.")]
        public int AccountId { get; set; }

        [DataMember]
        public string BankName { get; set; }

        [DataMember]
        public string AccountType { get; set; }

        [DataMember]
        public string AccountHolder { get; set; }

        [DataMember]
        public string BeneficiaryCode { get; set; }

    }
}