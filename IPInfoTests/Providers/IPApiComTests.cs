using IPInfo;
using IPInfo.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace IPInfoTests.Providers
{
    /// <summary>
    /// Unit tests for the IPApiCom provider.
    /// </summary>
    [TestClass]
    public class IPApiComTests : GeoProviderTestBase
    {
        [TestInitialize]
        public void TestInit()
        {
            TestData = TestDataHelper.GetIPApiComSuccessData();
            ErrorData = TestDataHelper.GetIPApiComErrorData();
        }

        [TestMethod]
        public void TestGetSupportedFormats()
        {
            var expectedFormats = new List<ServiceResponseFormat> { ServiceResponseFormat.JSON, ServiceResponseFormat.XML };
            TestGetSupportedFormats(expectedFormats);
        }

        [TestMethod]
        public new void TestGetGeoDataJson()
        {
            base.TestGetGeoDataJson();
        }

        [TestMethod]
        public new void TestGetGeoDataXml()
        {
            base.TestGetGeoDataXml();
        }

        [TestMethod]
        public new void TestBadInputJson()
        {
            base.TestBadInputJson();
        }

        [TestMethod]
        public new void TestBadInputXml()
        {
            base.TestBadInputXml();
        }

        protected override IGeoProvider GetProvider()
        {
            var providerConfig = TestDataHelper.GetIPApiComConfiguration();
            return new IPApiCom(providerConfig);
        }

    }
}
