namespace IPInfo
{
    /// <summary>
    /// A simple class for returning geographical data for an IP address.
    /// </summary>
    public class IPGeoData
    {
        public string DataSource { get; set; }
        public bool Success { get; set; }
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string IPAddress { get; set; }
        public string RawResponse { get; set; }
        public ServiceResponseFormat RawResponseFormat { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string MetroCode { get; set; }
        public string AreaCode { get; set; }
        public string TimeZone { get; set; }
    }
}
