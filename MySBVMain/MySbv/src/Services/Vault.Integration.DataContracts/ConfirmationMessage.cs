using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Vault.Integration.DataContracts
{
    public class ConfirmationMessage
    {
        [DataMember, XmlElement]
        public string SafeId { get; set; }

        [DataMember, XmlElement]
        public string BagBarcode { get; set; }

        [DataMember, XmlElement]
        public DateTime Date { get; set; }
        
        [DataMember, XmlElement]
        public int Declared10 { get; set; }

        [DataMember, XmlElement]
        public int Declared20 { get; set; }

        [DataMember, XmlElement]
        public int Declared50 { get; set; }

        [DataMember, XmlElement]
        public int Declared100 { get; set; }

        [DataMember, XmlElement]
        public int Declared200 { get; set; }

        [DataMember, XmlElement]
        public int Counted10 { get; set; }

        [DataMember, XmlElement]
        public int Counted20 { get; set; }

        [DataMember, XmlElement]
        public int Counted50 { get; set; }

        [DataMember, XmlElement]
        public int Counted100 { get; set; }

        [DataMember, XmlElement]
        public int Counted200 { get; set; }

        [DataMember, XmlElement]
        public float CountedValue { get; set; }

        [DataMember, XmlElement]
        public float DeclaredValue { get; set; }
    }
}
