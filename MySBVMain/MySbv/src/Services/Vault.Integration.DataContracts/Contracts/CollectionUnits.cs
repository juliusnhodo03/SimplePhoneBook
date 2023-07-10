using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Vault.Integration.DataContracts.Contracts
{
    /// <summary>
    /// </summary>
    [DataContract(Namespace = "http://www.sbv.co.za/deposit")]
    public class CollectionUnits
    {
        /// <summary>
        /// </summary>
        [DataMember, XmlElement]
        public List<CollectionUnit> CollectionUnit { get; set; }
    }

    /// <summary>
    /// </summary>
    [DataContract(Namespace = "http://www.sbv.co.za/deposit")]
    public class CollectionUnit
    {
        /// <summary>
        /// </summary>
        [DataMember, XmlAttribute]
        public string Type { get; set; }

        /// <summary>
        /// </summary>
        [DataMember, XmlText]
        public string Value { get; set; }
    }
}