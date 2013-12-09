using System.Linq;
using IPInfo.Configuration;
using IPInfo.Providers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace IPInfo
{
    /// <summary>
    /// Manages the usage of multiple providers of geographical data for IP addresses.
    /// </summary>
    public class IPGeoManager
    {
        private const ServiceResponseFormat DefaultFormat = ServiceResponseFormat.JSON;

        /// <summary>
        /// Gets geographic data for an IP address from any available provider, using the specified format
        /// </summary>
        /// <param name="ipAddress">A valid IP address</param>
        /// <param name="format">The desired response format</param>
        /// <returns>Returns the parsed data or null if no providers were available.</returns>
        public IPGeoData GetGeoData(string ipAddress, ServiceResponseFormat format = DefaultFormat)
        {
            // Validate IP address
            IPAddress parsedAddress;
            var validIP = IPAddress.TryParse(ipAddress, out parsedAddress);
            if (!validIP) throw new ArgumentException("The IP address provided is not valid.", ipAddress);

            // Try each provider until a successful call is made
            var providers = GetProviders(format);
            foreach (var provider in providers)
            {
                // Call provider
                try
                {
                    var ipGeoData = provider.GetGeoData(ipAddress, format);
                    if (ipGeoData != null && ipGeoData.Success) return ipGeoData;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(String.Format("Error while attempting to call {0}: {1}", provider.ProviderName, ex.Message));
                }
            }

            // At this point, all available providers have been attempted unsuccessfully.
            return null;
        }

        /// <summary>
        /// Gets instances of the providers that support the specified format.
        /// </summary>
        private IEnumerable<IGeoProvider> GetProviders(ServiceResponseFormat format)
        {
            var configuredProviders = ConfigurationManager.GetProviderConfiguration();
            return from configuredProvider in configuredProviders
                   where configuredProvider.SupportedFormats.Contains(format.ToString())
                   select CreateProvider(configuredProvider) into provider
                   where provider != null
                   select provider;
        }

        /// <summary>
        /// Creates an instance of a provider based on the given configuration.
        /// </summary>
        /// <remarks>Returns null if the provider cannot be instantiated.</remarks>
        private IGeoProvider CreateProvider(ProviderElement configuredProvider)
        {
            try
            {
                var objectHandle = Activator.CreateInstance("IPInfo", "IPInfo.Providers." + configuredProvider.ProviderClass, false, 0, null, new object[] { configuredProvider }, null, null, null);
                var provider = (IGeoProvider)objectHandle.Unwrap();
                return provider;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error during provider creation: " + ex.Message);
                if (ex.InnerException != null) Debug.WriteLine("Inner exception: " + ex.InnerException.Message);
                return null;
            }
        }
    }
}
