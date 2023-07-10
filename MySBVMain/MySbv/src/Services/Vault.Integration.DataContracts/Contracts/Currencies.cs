using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Vault.Integration.DataContracts.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract(Namespace = "http://www.sbv.co.za/deposit")]
    public class Currencies
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlElement]
        public Denominations Denominations { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract(Namespace = "http://www.sbv.co.za/deposit")]
    public class Denominations
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlAttribute]
        public string CurrencyCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlAttribute]
        public decimal TotalValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlElement]
        public FIT Fit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlElement]
        public UnFIT UnFit { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract(Namespace = "http://www.sbv.co.za/deposit")]
    public class FIT
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlAttribute]
        public decimal Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlElement]
        public List<Denomination> Denomination { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract(Namespace = "http://www.sbv.co.za/deposit")]
    public class UnFIT
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlAttribute]
        public decimal Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlElement]
        public List<Denomination> Denomination { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract(Namespace = "http://www.sbv.co.za/deposit")]
    public class Denomination
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlAttribute]
        public int Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlAttribute]
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlText]
        public int Count { get; set; }
    }
}