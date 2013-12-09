using IPInfo.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Xml.Linq;

namespace IPInfo.Providers
{
    /// <summary>
    /// Provider for Smart-IP.net
    /// </summary>
    /// <example>
    /// Send HTTP GET requests to: smart-ip.net/geoip-{format}/{ip_or_hostname}
    /// The API supports both HTTP and HTTPS. Supported formats are xml or json.
    /// </example>
    class SmartIPNet : GeoProviderBase
    {
        public SmartIPNet(ProviderElement config) : base(config) { }

        /// <summary>
        /// Parse the provider response into an IPGeoData instance
        /// </summary>
        /// <param name="response">The provider response as a string.</param>
        /// <param name="format">The format of the provider response.</param>
        /// <returns>Parsed IP geographical data.</returns>
        protected override IPGeoData ParseResponse(string response, ServiceResponseFormat format)
        {
            var data = new IPGeoData { DataSource = ProviderName, RawResponse = response, RawResponseFormat = format };
            object parsedResponse;
            switch (format)
            {
                case ServiceResponseFormat.JSON:
                    parsedResponse = JObject.Parse(response);
                    break;
                case ServiceResponseFormat.XML:
                    var xml = XDocument.Parse(response);
                    parsedResponse = xml.Element(XmlRootElementName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("format", String.Format("{0} format cannot be parsed.", format));
            }

            data.StatusMessage = GetStringDataFieldByName(parsedResponse, "error");
            data.Success = String.IsNullOrEmpty(data.StatusMessage);
            data.AreaCode = null;
            data.City = GetStringDataFieldByName(parsedResponse, "city");
            data.CountryCode = GetStringDataFieldByName(parsedResponse, "countryCode");
            data.CountryName = GetStringDataFieldByName(parsedResponse, "countryName");
            data.IPAddress = GetStringDataFieldByName(parsedResponse, "host");
            data.Latitude = GetDoubleDataFieldByName(parsedResponse, "latitude");
            data.Longitude = GetDoubleDataFieldByName(parsedResponse, "longitude");
            data.MetroCode = null;
            data.PostalCode = null;
            data.RegionCode = null;
            data.RegionName = GetStringDataFieldByName(parsedResponse, "region");
            data.TimeZone = GetStringDataFieldByName(parsedResponse, "timezone");

            return data;
        }
    }
}
