using IPInfo.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Xml.Linq;

namespace IPInfo.Providers
{
    /// <summary>
    /// Provider for ip-api.com
    /// </summary>
    /// <example>
    /// Send HTTP GET requests to: ip-api.com/{format}/{ip_or_hostname}
    /// Supported formats are csv, xml, json, line (newline separated) or php (serialized PHP).
    /// </example>
    class IPApiCom : GeoProviderBase
    {
        public IPApiCom(ProviderElement config) : base(config) { }

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

            data.StatusCode = GetStringDataFieldByName(parsedResponse, "status");
            data.Success = data.StatusCode == "success";
            data.StatusMessage = GetStringDataFieldByName(parsedResponse, "message");
            data.IPAddress = GetStringDataFieldByName(parsedResponse, "query");
            data.AreaCode = null;
            data.City = GetStringDataFieldByName(parsedResponse, "city");
            data.CountryCode = GetStringDataFieldByName(parsedResponse, "countryCode");
            data.CountryName = GetStringDataFieldByName(parsedResponse, "country");
            data.Latitude = GetDoubleDataFieldByName(parsedResponse, "lat");
            data.Longitude = GetDoubleDataFieldByName(parsedResponse, "lon");
            data.MetroCode = null;
            data.PostalCode = GetStringDataFieldByName(parsedResponse, "zip");
            data.RegionCode = GetStringDataFieldByName(parsedResponse, "region");
            data.RegionName = GetStringDataFieldByName(parsedResponse, "regionName");
            data.TimeZone = GetStringDataFieldByName(parsedResponse, "timezone");

            return data;
        }
    }
}
