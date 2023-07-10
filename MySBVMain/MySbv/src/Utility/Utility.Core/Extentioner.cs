using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Utility.Core
{
	/// <summary>
	/// Developed by JULIUS NHODO
	/// Date: 30/09/2014
	/// </summary>
	public static class Extentioner
	{
		/// <summary>
		///     Converts an Object to an XML file as string
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public static string ConvertToXml<T>(this T request)
		{
			//Represents an XML document,
			var xmlDoc = new XmlDocument();

			// Initializes a new instance of the XmlDocument class.          
			var xmlSerializer = new XmlSerializer(request.GetType());

			// Creates a stream whose backing store is memory. 
			using (var xmlStream = new MemoryStream())
			{
				xmlSerializer.Serialize(xmlStream, request);

				xmlStream.Position = 0;

				//Loads the XML document from the specified string.
				xmlDoc.Load(xmlStream);

				return xmlDoc.InnerXml;
			}
		}

		/// <summary>
		/// deserialize json string to a specific object.
		/// </summary>
		/// <param name="jsonString"></param>
		/// <returns></returns>
		public static T JsonDeserializer<T>(this string jsonString)
		{
			var response = Activator.CreateInstance<T>(); 
			using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonString)))
			{
				var serializer = new DataContractJsonSerializer(typeof(T));
				response = (T)serializer.ReadObject(ms); 
			}
			return response;
		}

		/// <summary>
		/// Serialize object to Json string
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public static string JsonSerializer<T>(this T request) 
		{
			var stream = new MemoryStream();
			var serializer = new DataContractJsonSerializer(typeof(T));

			serializer.WriteObject(stream, request);

			stream.Position = 0;
			var streamReader = new StreamReader(stream);

			return streamReader.ReadToEnd();
		}

	}
}