using System.Collections.Generic;

namespace IPInfo.Providers
{
    /// <summary>
    /// A common interface for all providers.
    /// </summary>
    public interface IGeoProvider
    {
        string ProviderName { get; }
        IEnumerable<ServiceResponseFormat> GetSupportedFormats();
        IPGeoData GetGeoData(string ipAddress, ServiceResponseFormat format);
    }
}
