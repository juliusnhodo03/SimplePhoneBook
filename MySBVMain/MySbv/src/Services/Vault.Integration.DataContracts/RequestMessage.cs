using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Vault.Integration.DataContracts.Contracts;

namespace Vault.Integration.DataContracts
{
    /// <summary>
    /// A Message sent to the web service
    /// </summary>
    [XmlRoot("RequestMessage")]
    [DataContract(Namespace = "http://www.sbv.co.za/Deposit")]
    public class RequestMessage
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlElement]
        public Guid UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlElement]
        public string BeneficiaryCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlElement]
        public string CitCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlElement]
        public string DeviceSerial { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlElement]
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlElement]
        public string ErrorCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlElement]
        public string ItramsReference { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlElement]
        public string UserReferance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlElement]
        public TransactionType TransactionType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlElement]
        public Users Users { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlElement]
        public CollectionUnits CollectionUnits { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //[DataMember, XmlElement]
        //public DeviceSerialNumbers DeviceSerialNumbers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlElement]
        public Currencies Currencies { get; set; }

    }
}
