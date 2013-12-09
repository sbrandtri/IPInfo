using IPInfo;
using IPInfo.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace IPInfoTests.Providers
{
    /// <summary>
    /// Unit tests for the IPInfoDBCom provider.
    /// </summary>
    [TestClass]
    public class IPInfoDBComTests : GeoProviderTestBase
    {
        [TestInitialize]
        public void TestInit()
        {
            TestData = TestDataHelper.GetIPInfoDBComSuccessData();
            ErrorData = TestDataHelper.GetIPInfoDBComErrorData();
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
            var providerConfig = TestDataHelper.GetIPInfoDBComConfiguration();
            return new IPInfoDBCom(providerConfig);
        }

    }
}
