using System.Configuration;

namespace IPInfo.Configuration
{
    /// <summary>
    /// A single provider configuration
    /// </summary>
    class ProviderElement : ConfigurationElement
    {
        [ConfigurationProperty("providerClass", IsRequired = true, IsKey = true)]
        public string ProviderClass
        {
            get { return base["providerClass"] as string; }
            set { base["providerClass"] = value; }
        }

        [ConfigurationProperty("priority", IsRequired = true, IsKey = true)]
        public int Priority
        {
            get { return (int)base["priority"]; }
            set { base["priority"] = value; }
        }

        [ConfigurationProperty("providerName", IsRequired = true, IsKey = true)]
        public string ProviderName
        {
            get { return base["providerName"] as string; }
            set { base["providerName"] = value; }
        }

        [ConfigurationProperty("serviceUrl", IsRequired = true, IsKey = false)]
        public string ServiceUrl
        {
            get { return base["serviceUrl"] as string; }
            set { base["serviceUrl"] = value; }
        }

        [ConfigurationProperty("supportedFormats", IsRequired = true, IsKey = false)]
        public string SupportedFormats
        {
            get { return base["supportedFormats"] as string; }
            set { base["supportedFormats"] = value; }
        }

        [ConfigurationProperty("xmlRootElementName", IsRequired = false, IsKey = false)]
        public string XmlRootElementName
        {
            get { return base["xmlRootElementName"] as string; }
            set { base["xmlRootElementName"] = value; }
        }

        [ConfigurationProperty("parseErrorResponses", IsRequired = false, IsKey = false)]
        public bool ParseErrorResponses
        {
            get { return (bool)base["parseErrorResponses"]; }
            set { base["parseErrorResponses"] = value; }
        }

        [ConfigurationProperty("timeout", IsRequired = false, IsKey = false)]
        public int Timeout
        {
            get { return (int)base["timeout"]; }
            set { base["timeout"] = value; }
        }

        [ConfigurationProperty("apiKey", IsRequired = false, IsKey = false)]
        public string ApiKey
        {
            get { return base["apiKey"] as string; }
            set { base["apiKey"] = value; }
        }
    }
}