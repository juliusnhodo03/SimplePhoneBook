using System.Runtime.Serialization;

namespace Vault.Integration.Service.Contracts
{
    [DataContract(Namespace = "http://www.sbv.co.za/Deposit")]
    public class DepositResult
    {
        [DataMember]
        public string Status { get; set; }

    }
}
