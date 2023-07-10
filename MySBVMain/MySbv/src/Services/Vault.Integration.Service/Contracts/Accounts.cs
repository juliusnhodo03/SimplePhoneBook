using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Vault.Integration.Service.Contracts
{
    [DataContract(Namespace = "http://www.sbv.co.za/Deposit")]
    public class Accounts
    {
        [DataMember]
        public List<AccountInformation> AccountsCollection { get; set; }
    }
}