using IPInfo.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IPInfo.Providers
{
    /// <summary>
    /// Provider for telize.com
    /// </summary>
    /// <example>
    /// Send HTTP GET requests to: www.telize.com/geoip/{ip_or_hostname}
    /// Only JSON is supported.
    /// </example>
    class TelizeCom : GeoProviderBase
    {
        public TelizeCom(ProviderElement config) : base(config) { }

        /// <summary>
        /// Parse the provider response into an IPGeoData instance
        /// </summary>
        /// <param name="response">The provider response as a string.</param>
        /// <param name="format">The format of the provider response.</param>
        /// <returns>Parsed IP geographical data.</returns>
        protected override IPGeoData ParseResponse(string response, ServiceResponseFormat format)
        {
            var data = new IPGeoData { DataSource = ProviderName, RawResponse = response, RawResponseFormat = format };
            switch (format)
            {
                case ServiceResponseFormat.JSON:
                    var json = JObject.Parse(response);
                    data.Success = true;
                    data.StatusMessage = null;
                    data.AreaCode = (string)json["area_code"];
                    data.City = (string)json["city"];
                    data.CountryCode = (string)json["country_code"];
                    data.CountryName = (string)json["country"];
                    data.IPAddress = (string)json["ip"];
                    data.Latitude = (double)json["latitude"];
                    data.Longitude = (double)json["longitude"];
                    data.MetroCode = (string)json["dma_code"];
                    data.PostalCode = (string)json["postal_code"];
                    data.RegionCode = (string)json["region_code"];
                    data.RegionName = (string)json["region"];
                    data.TimeZone = (string)json["timezone"];
                    break;
            }
            return data;
        }

        /// <summary>
        /// Parse the provider's error response into an IPGeoData instance.
        /// </summary>
        /// <remarks>
        /// By default, this method is not implemented. Providers should override this method if error data is returned 
        /// as response data in the requested format. Set ParseErrorResponses to true in the provider configuration if implemented.
        /// </remarks>
        /// <param name="response">The provider response as a string.</param>
        /// <param name="format">The format of the provider response.</param>
        /// <returns>Parsed error data.</returns>
        protected override IPGeoData ParseErrorResponse(string response, ServiceResponseFormat format)
        {
            var data = new IPGeoData { DataSource = ProviderName, Success = false, RawResponse = response, RawResponseFormat = format };
            switch (format)
            {
                case ServiceResponseFormat.JSON:
                    try
                    {
                        var json = JObject.Parse(response);
                        data.StatusCode = (string) json["code"];
                        data.StatusMessage = (string) json["message"];
                    }
                    catch (JsonReaderException)
                    {
                        // Since the response could not actually be parsed as JSON, it could be anything.
                        data.RawResponse = null;
                    }
                    break;
            }
            return data;
        }
    }
}
