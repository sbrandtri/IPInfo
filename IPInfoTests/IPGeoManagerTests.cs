using IPInfo;
using IPInfoTests.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;

namespace IPInfoTests
{
    /// <summary>
    /// Unit tests for the IPGeoManager class.
    /// </summary>
    [TestClass]
    public class IPGeoManagerTests
    {
        /// <summary>
        /// Tests that IPGeoManager returns the correct results for the following scenario:
        ///     * Two providers are configured
        ///     * The first provider times out before a response is received
        ///     * The second provider is called successfully
        /// The expected results is that results from the second provider are returned.
        /// </summary>
        [TestMethod]
        public void TestGetGeoData_ShortTimeout()
        {
            var expectedData = TestDataHelper.GetTelizeComSuccessData();

            IPGeoData actualData = null;
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (directoryName != null)
            {
                var testConfigFile = Path.Combine(directoryName, @"..\..\TestConfigFiles\ShortTimeout.App.config");
                using (AppConfig.Change(testConfigFile))
                {
                    var ipGeoManager = new IPGeoManager();
                    actualData = ipGeoManager.GetGeoData(TestDataHelper.TestIP);
                }
            }

            GeoProviderTestBase.PerformIPGeoDataAssertions(expectedData, actualData);
        }

        /// <summary>
        /// Tests that IPGeoManager returns the correct results for the following scenario:
        ///     * Four providers are configured
        ///     * The first provider times out before a response is received
        ///     * The second provider returns a failure
        ///     * The third provider throws an exception
        ///     * The fourth provider is called successfully
        /// The expected result is that results from the fourth provider are returned.
        /// </summary>
        [TestMethod]
        public void TestGetGeoData_TimeoutFailureException()
        {
            var expectedData = TestDataHelper.GetSmartIPNetSuccessData();

            IPGeoData actualData = null;
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (directoryName != null)
            {
                var testConfigFile = Path.Combine(directoryName, @"..\..\TestConfigFiles\TimeoutFailureException.App.config");
                using (AppConfig.Change(testConfigFile))
                {
                    var ipGeoManager = new IPGeoManager();
                    actualData = ipGeoManager.GetGeoData(TestDataHelper.TestIP);
                }
            }

            GeoProviderTestBase.PerformIPGeoDataAssertions(expectedData, actualData);
        }
    }
}
