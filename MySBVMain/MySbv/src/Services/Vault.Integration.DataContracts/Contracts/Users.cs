using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Vault.Integration.DataContracts.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract(Namespace = "http://www.sbv.co.za/deposit")]
    public class Users
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlElement]
        public User User { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract(Namespace = "http://www.sbv.co.za/deposit")]
    public class User
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlAttribute]
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlText]
        public string Value { get; set; }
    }
}