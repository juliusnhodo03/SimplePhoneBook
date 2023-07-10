using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Domain.Logging;
using Domain.Serializer;

namespace Infrastructure.Serializer
{
    [Export(typeof(ISerializer))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class XmlSerializer : ISerializer
    {
        #region Fields

        private readonly ILogger _logger;

        #endregion

        #region Constructor

		[ImportingConstructor]
        public XmlSerializer(ILogger logger)
        {
            _logger = logger;
        }

        #endregion

        #region ISerializer

        public string Serialize<T>(T objectToSerialize) where T : class
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    var writer = new StreamWriter(stream, Encoding.Unicode);
                    var serializer = new System.Xml.Serialization.XmlSerializer(typeof (T));
                    serializer.Serialize(writer, objectToSerialize);

                    var count = (int) stream.Length;
                    var arr = new byte[count];
                    stream.Seek(0, SeekOrigin.Begin);
                    //copy stream contents in byte array
                    stream.Read(arr, 0, count);
                    var utf = new UnicodeEncoding();
                    writer.Dispose();
                    return utf.GetString(arr).Trim();
                }
            }
            catch (SerializationException exception)
            {
                _logger.LogException(exception);
                return string.Empty;
            }
        }

        public T Deserialize<T>(object objectToDesirialize) where T : class
        {
            try
            {
				byte[] bytes = Encoding.Unicode.GetBytes((string)objectToDesirialize);
				using (var stream = new MemoryStream(bytes))
				{
					var deserializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
					return (T)deserializer.Deserialize(stream);
				}
            }
            catch (Exception exception)
            {
                _logger.LogException(exception);
                return null;
            }
        }

        #endregion
    }
}