using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Vault.Integration.DataContracts;

namespace Vault.Integration.WebService
{
    /// <summary>
    /// </summary>
    [ServiceContract(Namespace = "http://www.sbv.co.za/Deposit")]
    public interface IVaultService
    {
        /// <summary>
        /// </summary>
        /// <param name="citCode"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "api/GetBeneficiaries/{citCode}")]
        [OperationContract]
        IEnumerable<Beneficiary> GetBeneficiaries(string citCode);

        /// <summary>
        /// </summary>
        /// <param name="cashDepost"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "api/SubmitRequest", RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        [XmlSerializerFormat]
        List<ValidationError> SubmitRequest(RequestMessage cashDepost);


        /// <summary>
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "api/VerifyBag/{serialNumber}")]
        [OperationContract]
        ValidationError VerifyBag(string serialNumber);
    }
}