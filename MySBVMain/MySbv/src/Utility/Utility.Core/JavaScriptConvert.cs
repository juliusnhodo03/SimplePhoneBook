using System.IO;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Utility.Core
{
	public static class JavaScriptConvert
	{
		/// <summary>
		/// Serialize object
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static IHtmlString SerializeObject(object value)
		{
			using (var stringWriter = new StringWriter())
			using (var jsonWriter = new JsonTextWriter(stringWriter))
			{
				var serializer = new JsonSerializer
				{
					// Let's use default Casing as is common practice in JavaScript
					ContractResolver = new DefaultContractResolver()
				};
				// We don't want quotes around object names
				jsonWriter.QuoteName = false;
				serializer.Serialize(jsonWriter, value);

				return new HtmlString(stringWriter.ToString());
			}
		}

		/// <summary>
		/// Reverts an Object to its Type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="json"></param>
		/// <returns></returns>
		public static T DeSerializeObject<T>(JObject json)
		{
			var item = JsonConvert.DeserializeObject<T>(json.ToString());
			return item;
		}
	}
}
