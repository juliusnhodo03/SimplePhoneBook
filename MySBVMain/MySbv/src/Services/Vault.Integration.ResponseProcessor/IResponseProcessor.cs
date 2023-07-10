using Domain.Data.Model;
using Utility.Core;
using Vault.Integration.DataContracts;

namespace Vault.Integration.ResponseProcessor
{
    public interface IResponseProcessor
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="confirmationMessage"></param>
        /// <returns></returns>
        MethodResult<ConfirmationMessage> SubmitResponse(ConfirmationMessage confirmationMessage);

    }
}