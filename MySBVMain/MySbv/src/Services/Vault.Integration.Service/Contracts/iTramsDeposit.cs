using System;
using System.Runtime.Serialization;

namespace Vault.Integration.Service.Contracts
{
    [DataContract(Namespace = "http://www.sbv.co.za/Deposit")]
    // ReSharper disable once InconsistentNaming
    public class iTramsDeposit
    {
        [DataMember]
        public string CitCode { get; set; }

        [DataMember]
        public string DeviceSerialNumber { get; set; }

        [DataMember]
        [Obsolete("Use Beneficiary Code instead of AccountId. AccountId will be removed in future updates.")]
        public int AccountId { get; set; }

        [DataMember]
        public string BeneficiaryCode { get; set; }

        [DataMember]
        public bool IsNewBag { get; set; }

        [DataMember]
// ReSharper disable once InconsistentNaming
        public string iTramsReference { get; set; }

        [DataMember]
        public string NoteBagSealNumber { get; set; }

        [DataMember]
        public string CoinBagSealNumber { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public double TotalAmount { get; set; }

        [DataMember]
        public int ZAR5 { get; set; }

        [DataMember]
        public int ZAR10 { get; set; }

        [DataMember]
        public int ZAR20 { get; set; }

        [DataMember]
        public int ZAR50 { get; set; }

        [DataMember]
        public int ZAR100 { get; set; }

        [DataMember]
        public int ZAR200 { get; set; }

        [DataMember]
        public int ZAR500 { get; set; }

        [DataMember]
        public int ZAR1000 { get; set; }

        [DataMember]
        public int ZAR2000 { get; set; }

        [DataMember]
        public int ZAR5000 { get; set; }

        [DataMember]
        public int ZAR10000 { get; set; }

        [DataMember]
        public int ZAR20000 { get; set; }
    }
}