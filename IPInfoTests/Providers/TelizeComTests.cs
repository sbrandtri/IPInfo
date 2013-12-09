using IPInfo;
using IPInfo.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace IPInfoTests.Providers
{
    /// <summary>
    /// Unit tests for the TelizeCom provider.
    /// </summary>
    [TestClass]
    public class TelizeComTests : GeoProviderTestBase
    {
        [TestInitialize]
        public void TestInit()
        {
            TestData = TestDataHelper.GetTelizeComSuccessData();
            ErrorData = TestDataHelper.GetTelizeComErrorData();
        }

        [TestMethod]
        public void TestGetSupportedFormats()
        {
            var expectedFormats = new List<ServiceResponseFormat> { ServiceResponseFormat.JSON };
            TestGetSupportedFormats(expectedFormats);
        }

        [TestMethod]
        public new void TestGetGeoDataJson()
        {
            base.TestGetGeoDataJson();
        }

        // An exception is expected because telize.com doesn't support XML format.
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public new void TestGetGeoDataXml()
        {
            base.TestGetGeoDataXml();
        }

        [TestMethod]
        public new void TestBadInputJson()
        {
            base.TestBadInputJson();
        }

        // An exception is expected because telize.com doesn't support XML format.
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public new void TestBadInputXml()
        {
            base.TestBadInputXml();
        }

        protected override IGeoProvider GetProvider()
        {
            var providerConfig = TestDataHelper.GetTelizeComConfiguration();
            return new TelizeCom(providerConfig);
        }

    }
}
