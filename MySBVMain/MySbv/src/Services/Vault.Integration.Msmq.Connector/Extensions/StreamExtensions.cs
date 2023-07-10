using System;
using System.IO;
using Newtonsoft.Json;

namespace Vault.Integration.Msmq.Connector.Extensions
{
    /// <summary>
    /// 
    /// </summary>
	public static class StreamExtensions
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
		public static string ReadToEnd(this Stream stream)
		{
			var reader = new StreamReader(stream);
			return reader.ReadToEnd();
		}

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
		public static T ReadFromJson<T>(this Stream stream)
		{
			string json = stream.ReadToEnd();
			return JsonConvert.DeserializeObject<T>(json);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="messageType"></param>
        /// <returns></returns>
		public static object ReadToJson(this Stream stream, string messageType)
		{
			Type type = Type.GetType(messageType);
			string json = stream.ReadToEnd();
			return JsonConvert.DeserializeObject(json, type);
		}
	}
}