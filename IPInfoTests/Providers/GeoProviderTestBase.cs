using IPInfo;
using IPInfo.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace IPInfoTests.Providers
{
    /// <summary>
    /// Base unit test logic for all providers.
    /// </summary>
    public abstract class GeoProviderTestBase
    {
        protected IPGeoData TestData;
        protected IPGeoData ErrorData;

        /// <summary>
        /// Retrieves an instance of the provider under test.
        /// </summary>
        abstract protected IGeoProvider GetProvider();

        /// <summary>
        /// Tests that the GetSupportedFormats methods returns expected results.
        /// </summary>
        protected void TestGetSupportedFormats(IList<ServiceResponseFormat> expectedFormats)
        {
            var actualFormats = new List<ServiceResponseFormat>(GetProvider().GetSupportedFormats());

            Assert.AreEqual(expectedFormats.Count, actualFormats.Count);
            foreach (var format in expectedFormats)
            {
                Assert.IsTrue(actualFormats.Contains(format));
            }
        }

        /// <summary>
        /// Tests that GetGeoData returns expected results when requesting data in JSON format.
        /// </summary>
        protected void TestGetGeoDataJson()
        {
            var expectedData = TestData;
            expectedData.RawResponseFormat = ServiceResponseFormat.JSON;
            var actualData = GetProvider().GetGeoData(TestDataHelper.TestIP, ServiceResponseFormat.JSON);
            PerformIPGeoDataAssertions(expectedData, actualData);
            PerformJsonFormatAssertions(actualData);
        }

        /// <summary>
        /// Tests that GetGeoData returns expected results when requesting data in XML format.
        /// </summary>
        protected void TestGetGeoDataXml()
        {
            var expectedData = TestData;
            expectedData.RawResponseFormat = ServiceResponseFormat.XML;
            var actualData = GetProvider().GetGeoData(TestDataHelper.TestIP, ServiceResponseFormat.XML);
            PerformIPGeoDataAssertions(expectedData, actualData);
            PerformXmlFormatAssertions(actualData);
        }

        /// <summary>
        /// Tests that GetGeoData returns expected results when requesting data for a bad IP address in JSON format.
        /// </summary>
        protected void TestBadInputJson()
        {
            var expectedData = ErrorData;
            expectedData.RawResponseFormat = ServiceResponseFormat.JSON;
            var actualData = GetProvider().GetGeoData(TestDataHelper.BadIP, ServiceResponseFormat.JSON);
            PerformIPGeoDataAssertions(expectedData, actualData);
        }

        /// <summary>
        /// Tests that GetGeoData returns expected results when requesting data for a bad IP address in XML format.
        /// </summary>
        protected void TestBadInputXml()
        {
            var expectedData = ErrorData;
            expectedData.RawResponseFormat = ServiceResponseFormat.XML;
            var actualData = GetProvider().GetGeoData(TestDataHelper.BadIP, ServiceResponseFormat.XML);
            PerformIPGeoDataAssertions(expectedData, actualData);
        }

        /// <summary>
        /// Performs assertions on most properties of IPGeoData, comparing an expected outcome to actual data.
        /// </summary>
        public static void PerformIPGeoDataAssertions(IPGeoData expectedData, IPGeoData actualData)
        {
            Assert.AreEqual(expectedData.RawResponseFormat, actualData.RawResponseFormat);
            Assert.AreEqual(expectedData.Success, actualData.Success);
            Assert.AreEqual(expectedData.StatusCode, actualData.StatusCode);
            Assert.AreEqual(expectedData.StatusMessage, actualData.StatusMessage);
            Assert.AreEqual(expectedData.DataSource, actualData.DataSource);
            Assert.AreEqual(expectedData.IPAddress, actualData.IPAddress);
            Assert.AreEqual(expectedData.CountryCode, actualData.CountryCode);
            Assert.AreEqual(expectedData.CountryName, actualData.CountryName);
            Assert.AreEqual(expectedData.RegionCode, actualData.RegionCode);
            Assert.AreEqual(expectedData.RegionName, actualData.RegionName);
            Assert.AreEqual(expectedData.City, actualData.City);
            Assert.AreEqual(expectedData.PostalCode, actualData.PostalCode);
            Assert.AreEqual(expectedData.Latitude, actualData.Latitude);
            Assert.AreEqual(expectedData.Longitude, actualData.Longitude);
            Assert.AreEqual(expectedData.MetroCode, actualData.MetroCode);
            Assert.AreEqual(expectedData.AreaCode, actualData.AreaCode);
            Assert.AreEqual(expectedData.TimeZone, actualData.TimeZone);
        }

        /// <summary>
        /// Asserts that the raw response in the provided data is a valid JSON string.
        /// </summary>
        private static void PerformJsonFormatAssertions(IPGeoData actualData)
        {
            // Attempt to parse the raw response to make sure it is really a JSON string
            bool isJson;
            try
            {
                JObject.Parse(actualData.RawResponse);
                isJson = true;
            }
            catch (Exception)
            {
                isJson = false;
            }
            Assert.IsTrue(isJson, "The raw response is not a valid JSON string.");
        }

        /// <summary>
        /// Asserts that the raw response in the provided data is a valid XML string.
        /// </summary>
        private static void PerformXmlFormatAssertions(IPGeoData actualData)
        {
            // Attempt to parse the raw response to make sure it is really a XML string
            bool isXml;
            try
            {
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                XDocument.Parse(actualData.RawResponse);
                isXml = true;
            }
            catch (Exception)
            {
                isXml = false;
            }
            Assert.IsTrue(isXml, "The raw response is not a valid XML string.");
        }
    }
}
