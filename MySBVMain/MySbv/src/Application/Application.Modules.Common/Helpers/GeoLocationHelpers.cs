using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace Application.Modules.Common.Helpers
{
    public class GeoLocation
    {
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
    }

    public class GeoGeometry
    {
        public GeoLocation Location { get; set; }
    }

    public class GeoResult
    {
        public GeoGeometry Geometry { get; set; }
    }

    public class GeoResponse
    {
        public string Status { get; set; }
        public GeoResult[] Results { get; set; }
    }

    public class GeoLocationHelper
    {
        public static GeoGeometry GetCoordinates(string address)
        {
            const string url = "http://maps.googleapis.com/maps/api/geocode/json?address={0}&sensor=false";

            WebResponse response = null;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(string.Format(url, address));
                request.Method = "GET";
                response = request.GetResponse();
                {
                    string str = null;
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (var streamReader = new StreamReader(stream))
                        {
                            str = streamReader.ReadToEnd();
                        }
                    }

                    var geoResponse = JsonConvert.DeserializeObject<GeoResponse>(str);

                    return geoResponse.Results[0].Geometry;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}