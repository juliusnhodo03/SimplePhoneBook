using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Vault.Integration.DataContracts.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract(Namespace = "http://www.sbv.co.za/deposit")]
    public class TransactionType
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlAttribute]
        public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlText]
        public string Value { get; set; }
    }
}