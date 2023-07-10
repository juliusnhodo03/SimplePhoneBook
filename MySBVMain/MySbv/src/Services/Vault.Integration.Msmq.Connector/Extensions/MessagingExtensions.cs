using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Vault.Integration.Msmq.Connector.Extensions
{
    /// <summary>
    /// 
    /// </summary>
	public static class MessagingExtensions
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageObject"></param>
        /// <returns></returns>
		public static string GetMessageType(this object messageObject)
		{
			return messageObject.GetType().AssemblyQualifiedName;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageObject"></param>
        /// <returns></returns>
		public static string ToJsonString(this object messageObject)
		{
			return JsonConvert.SerializeObject(messageObject);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageObject"></param>
        /// <returns></returns>
		public static Stream ToJsonStream(this object messageObject)
		{
			string json = messageObject.ToJsonString();
			return new MemoryStream(Encoding.Default.GetBytes(json));
		}
	}
}