using IPInfo;
using IPInfo.Configuration;
using System;

namespace IPInfoTests
{
    /// <summary>
    /// A simple class for managing test data in one place.
    /// </summary>
    static class TestDataHelper
    {
        public const string TestIP = "74.125.224.66";
        public const string BadIP = "74.125.224.999";

        #region freegeoip.net
        public static IPGeoData GetFreeGeoIPNetSuccessData()
        {
            return new IPGeoData
            {
                Success = true,
                StatusCode = null,
                StatusMessage = null,
                DataSource = "freegeoip.net",
                IPAddress = TestIP,
                CountryCode = "US",
                CountryName = "United States",
                RegionCode = "CA",
                RegionName = "California",
                City = "Mountain View",
                PostalCode = "94043",
                Latitude = 37.4192,
                Longitude = -122.0574,
                MetroCode = "807",
                AreaCode = "650",
                TimeZone = null
            };
        }

        public static IPGeoData GetFreeGeoIPNetErrorData()
        {
            return new IPGeoData
            {
                Success = false,
                StatusCode = "NotFound",
                StatusMessage = "Not Found",
                DataSource = "freegeoip.net",
                IPAddress = BadIP,
                CountryCode = null,
                CountryName = null,
                RegionCode = null,
                RegionName = null,
                City = null,
                PostalCode = null,
                Latitude = null,
                Longitude = null,
                MetroCode = null,
                AreaCode = null,
                TimeZone = null
            };
        }

        public static ProviderElement GetFreeGeoIPNetConfiguration()
        {
            return new ProviderElement
            {
                ProviderClass = "FreeGeoIPNet",
                Priority = 1,
                ProviderName = "freegeoip.net",
                ServiceUrl = "http://freegeoip.net/{format}/{ip}",
                SupportedFormats = "JSON,XML",
                XmlRootElementName = "Response",
                ParseErrorResponses = false,
                Timeout = 1000
            };
        }
        #endregion
        #region ip-api.com
        public static IPGeoData GetIPApiComSuccessData()
        {
            return new IPGeoData
            {
                Success = true,
                StatusCode = "success",
                StatusMessage = null,
                DataSource = "ip-api.com",
                IPAddress = TestIP,
                CountryCode = "US",
                CountryName = "United States",
                RegionCode = "CA",
                RegionName = "California",
                City = "Mountain View",
                PostalCode = "94043",
                Latitude = 37.4192,
                Longitude = -122.0574,
                MetroCode = null,
                AreaCode = null,
                TimeZone = "America/Los_Angeles"
            };
        }

        public static IPGeoData GetIPApiComErrorData()
        {
            return new IPGeoData
            {
                Success = false,
                StatusCode = "fail",
                StatusMessage = "invalid query",
                DataSource = "ip-api.com",
                IPAddress = BadIP,
                CountryCode = null,
                CountryName = null,
                RegionCode = null,
                RegionName = null,
                City = null,
                PostalCode = null,
                Latitude = null,
                Longitude = null,
                MetroCode = null,
                AreaCode = null,
                TimeZone = null
            };
        }

        public static ProviderElement GetIPApiComConfiguration()
        {
            return new ProviderElement
            {
                ProviderClass = "IPApiCom",
                Priority = 3,
                ProviderName = "ip-api.com",
                ServiceUrl = "http://ip-api.com/{format}/{ip}",
                SupportedFormats = "JSON,XML",
                XmlRootElementName = "query",
                ParseErrorResponses = false,
                Timeout = 1000
            };
        }
        #endregion
        #region ipinfodb.com
        public static IPGeoData GetIPInfoDBComSuccessData()
        {
            return new IPGeoData
            {
                Success = true,
                StatusCode = "OK",
                StatusMessage = String.Empty,
                DataSource = "ipinfodb.com",
                IPAddress = TestIP,
                CountryCode = "US",
                CountryName = "UNITED STATES",
                RegionCode = null,
                RegionName = "CALIFORNIA",
                City = "MOUNTAIN VIEW",
                PostalCode = "94043",
                Latitude = 37.406,
                Longitude = -122.079,
                MetroCode = null,
                AreaCode = null,
                TimeZone = "-07:00"
            };
        }

        public static IPGeoData GetIPInfoDBComErrorData()
        {
            return new IPGeoData
            {
                Success = false,
                StatusCode = "ERROR",
                StatusMessage = "Invalid IP Address.",
                DataSource = "ipinfodb.com",
                IPAddress = BadIP,
                CountryCode = String.Empty,
                CountryName = String.Empty,
                RegionCode = null,
                RegionName = String.Empty,
                City = String.Empty,
                PostalCode = String.Empty,
                Latitude = 0,
                Longitude = 0,
                MetroCode = null,
                AreaCode = null,
                TimeZone = String.Empty
            };
        }

        public static ProviderElement GetIPInfoDBComConfiguration()
        {
            return new ProviderElement
            {
                ProviderClass = "IPInfoDBCom",
                Priority = 4,
                ProviderName = "ipinfodb.com",
                ServiceUrl = "http://api.ipinfodb.com/v3/ip-city/?key={apiKey}&ip={ip}&format={format}",
                SupportedFormats = "JSON,XML",
                XmlRootElementName = "Response",
                ParseErrorResponses = false,
                Timeout = 1000,
                ApiKey = "a663e2087cf9acf88144e778657dca1d49ad00448d85d652a5b090e81aed3554"
            };
        }
        #endregion
        #region smart-ip.net
        public static IPGeoData GetSmartIPNetSuccessData()
        {
            return new IPGeoData
            {
                Success = true,
                StatusCode = null,
                StatusMessage = null,
                DataSource = "smart-ip.net",
                IPAddress = TestIP,
                CountryCode = "US",
                CountryName = "United States",
                RegionCode = null,
                RegionName = "California",
                City = "Mountain View",
                PostalCode = null,
                Latitude = 37.4192,
                Longitude = -122.0570,
                MetroCode = null,
                AreaCode = null,
                TimeZone = "America/Los_Angeles"
            };
        }

        public static IPGeoData GetSmartIPNetErrorData()
        {
            return new IPGeoData
            {
                Success = false,
                StatusCode = null,
                StatusMessage = "Cannot resolve host name!",
                DataSource = "smart-ip.net",
                IPAddress = BadIP,
                CountryCode = null,
                CountryName = null,
                RegionCode = null,
                RegionName = null,
                City = null,
                PostalCode = null,
                Latitude = null,
                Longitude = null,
                MetroCode = null,
                AreaCode = null,
                TimeZone = null
            };
        }

        public static ProviderElement GetSmartIPNetConfiguration()
        {
            return new ProviderElement
            {
                ProviderClass = "SmartIPNet",
                Priority = 5,
                ProviderName = "smart-ip.net",
                ServiceUrl = "http://smart-ip.net/geoip-{format}/{ip}",
                SupportedFormats = "JSON,XML",
                XmlRootElementName = "geoip",
                ParseErrorResponses = false,
                Timeout = 1000
            };
        }
        #endregion
        #region telize.com
        public static IPGeoData GetTelizeComSuccessData()
        {
            return new IPGeoData
            {
                Success = true,
                StatusCode = null,
                StatusMessage = null,
                DataSource = "telize.com",
                IPAddress = TestIP,
                CountryCode = "US",
                CountryName = "United States",
                RegionCode = "CA",
                RegionName = "California",
                City = "Mountain View",
                PostalCode = "94043",
                Latitude = 37.4192,
                Longitude = -122.0574,
                MetroCode = "0",
                AreaCode = "0",
                TimeZone = "America/Los_Angeles"
            };
        }

        public static IPGeoData GetTelizeComErrorData()
        {
            return new IPGeoData
            {
                Success = false,
                StatusCode = "401",
                StatusMessage = "Input string is not a valid IP address",
                DataSource = "telize.com",
                IPAddress = BadIP,
                CountryCode = null,
                CountryName = null,
                RegionCode = null,
                RegionName = null,
                City = null,
                PostalCode = null,
                Latitude = null,
                Longitude = null,
                MetroCode = null,
                AreaCode = null,
                TimeZone = null
            };
        }

        public static ProviderElement GetTelizeComConfiguration()
        {
            return new ProviderElement
            {
                ProviderClass = "TelizeCom",
                Priority = 2,
                ProviderName = "telize.com",
                ServiceUrl = "http://www.telize.com/geoip/{ip}",
                SupportedFormats = "JSON",
                ParseErrorResponses = true,
                Timeout = 1000
            };
        }
        #endregion
    }
}
