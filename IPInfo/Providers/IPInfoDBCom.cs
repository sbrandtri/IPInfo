using IPInfo.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Xml.Linq;

namespace IPInfo.Providers
{
    /// <summary>
    /// Provider for IPInfoDB.com
    /// </summary>
    /// <example>
    /// Send HTTP GET requests to: api.ipinfodb.com/v3/ip-city/?key={api_key}&amp;ip={ip_or_hostname}&amp;format={format}
    /// Supported formats are raw, xml or json.
    /// </example>
    class IPInfoDBCom : GeoProviderBase
    {
        public IPInfoDBCom(ProviderElement config) : base(config) { }

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

            data.StatusCode = GetStringDataFieldByName(parsedResponse, "statusCode");
            data.Success = data.StatusCode == "OK";
            data.StatusMessage = GetStringDataFieldByName(parsedResponse, "statusMessage");
            data.AreaCode = null;
            data.City = GetStringDataFieldByName(parsedResponse, "cityName");
            data.CountryCode = GetStringDataFieldByName(parsedResponse, "countryCode");
            data.CountryName = GetStringDataFieldByName(parsedResponse, "countryName");
            data.IPAddress = GetStringDataFieldByName(parsedResponse, "ipAddress");
            data.Latitude = GetDoubleDataFieldByName(parsedResponse, "latitude");
            data.Longitude = GetDoubleDataFieldByName(parsedResponse, "longitude");
            data.MetroCode = null;
            data.PostalCode = GetStringDataFieldByName(parsedResponse, "zipCode");
            data.RegionCode = null;
            data.RegionName = GetStringDataFieldByName(parsedResponse, "regionName");
            data.TimeZone = GetStringDataFieldByName(parsedResponse, "timeZone");

            return data;
        }
    }
}
