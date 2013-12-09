using System.Linq;
using IPInfo.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace IPInfo.Providers
{
    /// <summary>
    /// Base provider class
    /// </summary>
    abstract class GeoProviderBase : IGeoProvider
    {
        public string ProviderName { get; private set; }
        protected string XmlRootElementName { get; private set; }
        private List<ServiceResponseFormat> SupportedFormats { get; set; }
        private string ServiceUrl { get; set; }
        private bool ParseErrorResponses { get; set; }
        private int Timeout { get; set; }
        private string ApiKey { get; set; }

        /// <summary>
        /// Parse the provider response into an IPGeoData instance
        /// </summary>
        /// <param name="response">The provider response as a string.</param>
        /// <param name="format">The format of the provider response.</param>
        /// <returns>Parsed IP geographical data.</returns>
        abstract protected IPGeoData ParseResponse(string response, ServiceResponseFormat format);

        /// <summary>
        /// The default constructor will not be used. Pass a configuration to create a new instance.
        /// </summary>
        protected GeoProviderBase() { }

        /// <summary>
        /// Create a new GeoProviderBase instance based on the specified configuration.
        /// </summary>
        /// <param name="config">A provider configuration.</param>
        protected GeoProviderBase(ProviderElement config)
        {
            ProviderName = config.ProviderName;
            ServiceUrl = config.ServiceUrl;
            XmlRootElementName = config.XmlRootElementName;
            ParseErrorResponses = config.ParseErrorResponses;
            Timeout = config.Timeout;
            SupportedFormats = ParseSupportedFormats(config.SupportedFormats);
            ApiKey = config.ApiKey;
        }

        /// <summary>
        /// Get the response formats supported by the provider.
        /// </summary>
        /// <returns>The supported response formats.</returns>
        public IEnumerable<ServiceResponseFormat> GetSupportedFormats()
        {
            return SupportedFormats;
        }

        /// <summary>
        /// Get the geographical data for the given IP address in the specified format.
        /// </summary>
        /// <param name="ipAddress">An IP address</param>
        /// <param name="format">The desired response format</param>
        /// <returns>Geographical data for the given IP address.</returns>
        public IPGeoData GetGeoData(string ipAddress, ServiceResponseFormat format)
        {
            if (!SupportedFormats.Contains(format))
                throw new ArgumentOutOfRangeException("format", String.Format("{0} does not support the {1} format.", ProviderName, format));

            IPGeoData geoData;
            try
            {
                // Make the call out to the data provider
                var response = GetResponse(BuildUrlString(ipAddress, format));

                // Parse the response data
                geoData = ParseResponse(response, format);
            }
            catch (WebException ex)
            {
                // If an error occurs, read the response stream, then determine the error data either based on the response or the exception.
                var httpResponse = (HttpWebResponse)ex.Response;
                var response = ReadResponseStream(httpResponse);
                httpResponse.Close();
                if (ParseErrorResponses)
                {
                    geoData = ParseErrorResponse(response, format);
                }
                else
                {
                    geoData = new IPGeoData { DataSource = ProviderName, RawResponseFormat = format };
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                    {
                        geoData.StatusCode = geoData.StatusCode ?? httpResponse.StatusCode.ToString();
                        geoData.StatusMessage = geoData.StatusMessage ?? httpResponse.StatusDescription;
                    }
                    else
                    {
                        geoData.StatusCode = "ERROR";
                        geoData.StatusMessage = geoData.StatusMessage ?? ex.Message;
                    }
                }
            }

            // Always echo back the IP address, even if the provider didn't return it.
            if (String.IsNullOrEmpty(geoData.IPAddress)) geoData.IPAddress = ipAddress;
            return geoData;
        }

        /// <summary>
        /// Build the URL string, based on the configured, tokenized service URL.
        /// </summary>
        /// <remarks>
        /// The configured service URL may contain the following 3 tokens, which will be replaced with the appropriate data or configuration:
        /// {ip}        The IP address to be used in the query
        /// {format}    The desired response format
        /// {apiKey}    The configured API key for the provider
        /// </remarks>
        /// <param name="ipAddress">The IP address to be used in the query</param>
        /// <param name="format">The desired response format</param>
        /// <returns>A URL for performing a query on a given IP address</returns>
        private string BuildUrlString(string ipAddress, ServiceResponseFormat format)
        {
            var urlFormat = new StringBuilder(ServiceUrl);
            urlFormat = urlFormat.Replace("{ip}", "{0}").Replace("{format}", "{1}").Replace("{apiKey}", "{2}");
            return String.Format(urlFormat.ToString(), ipAddress, format.ToString().ToLower(), ApiKey);
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
        protected virtual IPGeoData ParseErrorResponse(string response, ServiceResponseFormat format)
        {
            throw new NotImplementedException("ParseErrorResponse is not implemented.");
        }

        /// <summary>
        /// Get the response from a GET request to the service at the given URL.
        /// </summary>
        private string GetResponse(string url)
        {
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Timeout = Timeout;
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            var response = ReadResponseStream(httpResponse);
            httpResponse.Close();
            return response;
        }

        /// <summary>
        /// Reads a HTTP response stream.
        /// </summary>
        private static string ReadResponseStream(HttpWebResponse httpResponse)
        {
            if (httpResponse == null) throw new ArgumentNullException("httpResponse", "Cannot read response stream.");
            var responseStream = httpResponse.GetResponseStream();
            if (responseStream == null) { return null; }
            var readStream = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
            const int maxChars = 1024;
            var readBuffer = new char[maxChars];
            var response = new StringBuilder();
            int count;
            while ((count = readStream.Read(readBuffer, 0, maxChars)) > 0)
            {
                response.Append(readBuffer, 0, count);
            }
            readStream.Close();
            return response.ToString();
        }

        /// <summary>
        /// Gets a string field from a JSON or XML data structure.
        /// </summary>
        protected string GetStringDataFieldByName(object sourceData, string fieldName)
        {
            if (sourceData is JObject)
            {
                var json = sourceData as JObject;
                return (string)json[fieldName];
            }
            if (sourceData is XElement)
            {
                var xml = sourceData as XElement;
                var xElement = xml.Element(fieldName);
                if (xElement != null) return xElement.Value;
            }
            return null;
        }

        /// <summary>
        /// Gets a double field from a JSON or XML data structure.
        /// </summary>
        protected double? GetDoubleDataFieldByName(object sourceData, string fieldName)
        {
            if (sourceData is JObject)
            {
                var json = sourceData as JObject;
                return (double?)json[fieldName];
            }
            if (sourceData is XElement)
            {
                var xml = sourceData as XElement;
                var xElement = xml.Element(fieldName);
                if (xElement != null)
                {
                    double returnValue;
                    if (Double.TryParse(xElement.Value, out returnValue)) return returnValue;
                }
            }
            return null;
        }

        /// <summary>
        /// Parses the configured comma-delimited list of supported formats into a list of Enum values
        /// </summary>
        private List<ServiceResponseFormat> ParseSupportedFormats(string formatConfig)
        {
            var configuredFormats = formatConfig.Split(',');
            return (from configuredFormat in configuredFormats
                    where Enum.IsDefined(typeof(ServiceResponseFormat), configuredFormat)
                    select (ServiceResponseFormat)Enum.Parse(typeof(ServiceResponseFormat), configuredFormat)).ToList();
        }
    }
}
