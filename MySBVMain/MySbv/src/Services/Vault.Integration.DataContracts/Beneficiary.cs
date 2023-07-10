using System.Runtime.Serialization;

namespace Vault.Integration.DataContracts
{
    /// <summary>
    /// Represent the banking information of beneficiaries 
    /// excluding the account number
    /// </summary>
    [DataContract(Namespace = "http://www.sbv.co.za/Deposit")]
    public class Beneficiary
    {
        /// <summary>
        /// A unique code used to identify a beneficiary
        /// </summary>
        [DataMember]
        public string BeneficiaryCode { get; set; }

        /// <summary>
        /// Name of bank when beneficiary has a bank account
        /// </summary>
        [DataMember]
        public string BankName { get; set; }

        /// <summary>
        /// The name of the beneficiary as per
        /// what is in the bank
        /// </summary>
        [DataMember]
        public string AccountHolder { get; set; }

        /// <summary>
        /// The type of bank account
        /// </summary>
        [DataMember]
        public string AccountType { get; set; }

        /// <summary>
        /// Determines if the account is a default account.
        /// </summary>
        [DataMember]
        public bool IsDefault { get; set; }
    }
}