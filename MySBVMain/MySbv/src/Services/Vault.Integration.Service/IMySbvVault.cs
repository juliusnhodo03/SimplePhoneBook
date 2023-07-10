using System.ServiceModel;
using System.ServiceModel.Web;
using Vault.Integration.Service.Contracts;

namespace Vault.Integration.Service
{
    [ServiceContract(Namespace = "http://www.sbv.co.za/Deposit")]
    public interface IMySbvVault
    {
        [WebGet(UriTemplate = "api/Accounts/{citCode}")]
        [OperationContract]
        Accounts Accounts(string citCode);

        [WebInvoke(UriTemplate = "api/SubmitDeposit")]
        [OperationContract]
        DepositResult SubmitDeposit(iTramsDeposit cashDepost);
    }
}