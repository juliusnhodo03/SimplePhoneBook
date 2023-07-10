using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Vault.Integration.DataContracts.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract(Namespace = "http://www.sbv.co.za/deposit")]
    public class DeviceSerialNumbers
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<DeviceSerialNumber> DeviceSerialNumber { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract(Namespace = "http://www.sbv.co.za/deposit")]
    public class DeviceSerialNumber
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