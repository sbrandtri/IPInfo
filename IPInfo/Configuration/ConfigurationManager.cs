using System.Collections.Generic;

namespace IPInfo.Configuration
{
    static class ConfigurationManager
    {
        /// <summary>
        /// Retrieves the configured providers from the IPInfo section in the App.config or Web.config file
        /// </summary>
        /// <returns>Returns a collection of provider configurations, sorted by priority.</returns>
        public static ICollection<ProviderElement> GetProviderConfiguration()
        {
            var config = (ProviderConfigurationHandler)System.Configuration.ConfigurationManager.GetSection("IPInfo/providers");
            List<ProviderElement> providers;
            if (config == null || config.Providers.Count == 0)
            {
                providers = GetDefaultProviderConfiguration();
            }
            else
            {
                providers = new List<ProviderElement>(config.Providers);
            }
            providers.Sort(new ProviderPriorityComparer());
            return providers;
        }

        /// <summary>
        /// Returns default configurations for all providers, in the event that no configuration exists.
        /// </summary>
        /// <remarks>
        /// NOTE: ipinfodb.com is not included in the default configuration because it requires an API key.
        /// Either configure the providers in App.config or modify this method.
        /// </remarks>
        private static List<ProviderElement> GetDefaultProviderConfiguration()
        {
            const int defaultTimeout = 2000; // 2 seconds

            var providers = new List<ProviderElement>();

            var provider = new ProviderElement
            {
                ProviderClass = "FreeGeoIPNet",
                Priority = 1,
                ProviderName = "freegeoip.net",
                ServiceUrl = "http://freegeoip.net/{format}/{ip}",
                SupportedFormats = "JSON,XML",
                XmlRootElementName = "Response",
                ParseErrorResponses = false,
                Timeout = defaultTimeout
            };
            providers.Add(provider);

            provider = new ProviderElement
            {
                ProviderClass = "TelizeCom",
                Priority = 2,
                ProviderName = "telize.com",
                ServiceUrl = "http://www.telize.com/geoip/{ip}",
                SupportedFormats = "JSON",
                ParseErrorResponses = true,
                Timeout = defaultTimeout
            };
            providers.Add(provider);

            provider = new ProviderElement
            {
                ProviderClass = "IPApiCom",
                Priority = 3,
                ProviderName = "ip-api.com",
                ServiceUrl = "http://ip-api.com/{format}/{ip}",
                SupportedFormats = "JSON,XML",
                XmlRootElementName = "query",
                ParseErrorResponses = false,
                Timeout = defaultTimeout
            };
            providers.Add(provider);

            provider = new ProviderElement
            {
                ProviderClass = "SmartIPNet",
                Priority = 4,
                ProviderName = "smart-ip.net",
                ServiceUrl = "http://smart-ip.net/geoip-{format}/{ip}",
                SupportedFormats = "JSON,XML",
                XmlRootElementName = "geoip",
                ParseErrorResponses = false,
                Timeout = defaultTimeout
            };
            providers.Add(provider);

            // * Be sure to set ApiKey when uncommenting this!
            //provider = new ProviderElement
            //{
            //    ProviderClass = "IPInfoDBCom",
            //    Priority = 5,
            //    ProviderName = "ipinfodb.com",
            //    ServiceUrl = "http://api.ipinfodb.com/v3/ip-city/?key={apiKey}&ip={ip}&format={format}",
            //    SupportedFormats = "JSON,XML",
            //    XmlRootElementName = "Response",
            //    ParseErrorResponses = false,
            //    Timeout = defaultTimeout,
            //    ApiKey = ""
            //};
            //providers.Add(provider);

            return providers;
        }
    }
}
