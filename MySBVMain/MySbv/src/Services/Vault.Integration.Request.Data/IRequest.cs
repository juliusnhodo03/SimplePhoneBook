using Vault.Integration.DataContracts;

namespace Vault.Integration.Request.Data
{
	public interface IRequest
	{
		/// <summary>
		/// dumps Xml request to the database without apply validations
		/// </summary>
		/// <param name="request"></param>
		/// <param name="errorResults"></param>
		void DumpFailedXmlRequest(RequestMessage request, ValidationError[] errorResults);
	}
}