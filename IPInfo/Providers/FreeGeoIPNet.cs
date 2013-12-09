using IPInfo.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Xml.Linq;

namespace IPInfo.Providers
{
    /// <summary>
    /// Provider for freegeoip.net
    /// </summary>
    /// <example>
    /// Send HTTP GET requests to: freegeoip.net/{format}/{ip_or_hostname}
    /// The API supports both HTTP and HTTPS. Supported formats are csv, xml or json.
    /// </example>
    class FreeGeoIPNet : GeoProviderBase
    {
        public FreeGeoIPNet(ProviderElement config) : base(config) { }

        /// <summary>
        /// Parse the provider response into an IPGeoData instance
        /// </summary>
        /// <param name="response">The provider response as a string.</param>
        /// <param name="format">The format of the provider response.</param>
        /// <returns>Parsed IP geographical data.</returns>
        protected override IPGeoData ParseResponse(string response, ServiceResponseFormat format)
        {
            var data = new IPGeoData
            {
                DataSource = ProviderName,
                RawResponse = response,
                RawResponseFormat = format,
                Success = true,
                StatusCode = null,
                StatusMessage = null,
                TimeZone = null
            };

            // freegeoip.net returns data fields with different names in the JSON and XML versions of their API.
            object parsedResponse;
            switch (format)
            {
                case ServiceResponseFormat.JSON:
                    parsedResponse = JObject.Parse(response);
                    data.AreaCode = GetStringDataFieldByName(parsedResponse, "areacode");
                    data.City = GetStringDataFieldByName(parsedResponse, "city");
                    data.CountryCode = GetStringDataFieldByName(parsedResponse, "country_code");
                    data.CountryName = GetStringDataFieldByName(parsedResponse, "country_name");
                    data.IPAddress = GetStringDataFieldByName(parsedResponse, "ip");
                    data.Latitude = GetDoubleDataFieldByName(parsedResponse, "latitude");
                    data.Longitude = GetDoubleDataFieldByName(parsedResponse, "longitude");
                    data.MetroCode = GetStringDataFieldByName(parsedResponse, "metro_code");
                    data.PostalCode = GetStringDataFieldByName(parsedResponse, "zipcode");
                    data.RegionCode = GetStringDataFieldByName(parsedResponse, "region_code");
                    data.RegionName = GetStringDataFieldByName(parsedResponse, "region_name");
                    break;
                case ServiceResponseFormat.XML:
                    var xml = XDocument.Parse(response);
                    parsedResponse = xml.Element(XmlRootElementName);
                    data.AreaCode = GetStringDataFieldByName(parsedResponse, "AreaCode");
                    data.City = GetStringDataFieldByName(parsedResponse, "City");
                    data.CountryCode = GetStringDataFieldByName(parsedResponse, "CountryCode");
                    data.CountryName = GetStringDataFieldByName(parsedResponse, "CountryName");
                    data.IPAddress = GetStringDataFieldByName(parsedResponse, "Ip");
                    data.Latitude = GetDoubleDataFieldByName(parsedResponse, "Latitude");
                    data.Longitude = GetDoubleDataFieldByName(parsedResponse, "Longitude");
                    data.MetroCode = GetStringDataFieldByName(parsedResponse, "MetroCode");
                    data.PostalCode = GetStringDataFieldByName(parsedResponse, "ZipCode");
                    data.RegionCode = GetStringDataFieldByName(parsedResponse, "RegionCode");
                    data.RegionName = GetStringDataFieldByName(parsedResponse, "RegionName");
                    break;
                default:
                    throw new ArgumentOutOfRangeException("format", String.Format("{0} format cannot be parsed.", format));
            }

            return data;
        }
    }
}
