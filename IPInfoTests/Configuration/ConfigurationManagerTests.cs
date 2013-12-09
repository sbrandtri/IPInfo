using IPInfo.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace IPInfoTests.Configuration
{
    /// <summary>
    /// Unit tests for the ConfigurationManager class
    /// </summary>
    [TestClass]
    public class ConfigurationManagerTests
    {
        /// <summary>
        /// Load a configuration that has no providers configured. Expected to use the hard-coded default configuration.
        /// </summary>
        [TestMethod]
        public void TestGetProviderConfiguration_NoConfig()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (directoryName != null)
            {
                var testConfigFile = Path.Combine(directoryName, @"..\..\TestConfigFiles\NoConfig.App.config");
                using (AppConfig.Change(testConfigFile))
                {
                    IEnumerable<ProviderElement> providerConfigs = ConfigurationManager.GetProviderConfiguration();
                    Assert.AreEqual(4, ((IList<ProviderElement>)providerConfigs).Count);
                    Assert.AreEqual("FreeGeoIPNet", providerConfigs.ElementAt(0).ProviderClass);
                    Assert.AreEqual(1, providerConfigs.ElementAt(0).Priority);
                    Assert.AreEqual("telize.com", providerConfigs.ElementAt(1).ProviderName);
                    Assert.AreEqual(true, providerConfigs.ElementAt(1).ParseErrorResponses);
                    Assert.AreEqual("http://smart-ip.net/geoip-{format}/{ip}", providerConfigs.ElementAt(3).ServiceUrl);
                    Assert.AreEqual(2000, providerConfigs.ElementAt(3).Timeout);
                }
            }
        }

        /// <summary>
        /// Load a configuration that has one provider configured.
        /// </summary>
        [TestMethod]
        public void TestGetProviderConfiguration_OneProvider()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (directoryName != null)
            {
                var testConfigFile = Path.Combine(directoryName, @"..\..\TestConfigFiles\OneProvider.App.config");
                using (AppConfig.Change(testConfigFile))
                {
                    IEnumerable<ProviderElement> providerConfigs = ConfigurationManager.GetProviderConfiguration();
                    Assert.AreEqual(1, ((IList<ProviderElement>)providerConfigs).Count);
                    Assert.AreEqual("FreeGeoIPNet", providerConfigs.ElementAt(0).ProviderClass);
                    Assert.AreEqual(1, providerConfigs.ElementAt(0).Priority);
                }
            }
        }

        /// <summary>
        /// Load a configuration that has two providers configured.
        /// </summary>
        [TestMethod]
        public void TestGetProviderConfiguration_TwoProviders()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (directoryName != null)
            {
                var testConfigFile = Path.Combine(directoryName, @"..\..\TestConfigFiles\TwoProviders.App.config");
                using (AppConfig.Change(testConfigFile))
                {
                    IEnumerable<ProviderElement> providerConfigs = ConfigurationManager.GetProviderConfiguration();
                    Assert.AreEqual(2, ((IList<ProviderElement>)providerConfigs).Count);
                    Assert.AreEqual("FreeGeoIPNet", providerConfigs.ElementAt(0).ProviderClass);
                    Assert.AreEqual(1, providerConfigs.ElementAt(0).Priority);
                    Assert.AreEqual("telize.com", providerConfigs.ElementAt(1).ProviderName);
                    Assert.AreEqual(true, providerConfigs.ElementAt(1).ParseErrorResponses);
                    Assert.AreEqual(1000, providerConfigs.ElementAt(1).Timeout);
                }
            }
        }
    }
}
