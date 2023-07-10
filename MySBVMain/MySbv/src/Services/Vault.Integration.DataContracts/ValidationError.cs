using System.Net;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Vault.Integration.DataContracts
{
    /// <summary>
    ///     Used to store validation ErrorMessage and Error Code.
    /// </summary>
    ///
    [DataContract]
    public class ValidationError
    {
        /// <summary>
        ///     An HTTP Error Code
        /// </summary>
        /// 
        [DataMember, XmlElement]
        public HttpStatusCode ErrorCode { get; set; }

        /// <summary>
        ///     An error message
        /// </summary>
        /// 
        [DataMember, XmlElement]
        public string ErrorMessage { get; set; }
    }
}