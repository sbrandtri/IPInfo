using System.Configuration;
using IPInfo.Annotations;

namespace IPInfo.Configuration
{
    /// <summary>
    /// Implements a custom configuration section containing a collection of provider configurations.
    /// </summary>
    [UsedImplicitly]
    class ProviderConfigurationHandler : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true, IsKey = false, IsRequired = true)]
        public ProviderCollection Providers
        {
            get
            {
                return base[""] as ProviderCollection;
            }
            [UsedImplicitly] set
            {
                base[""] = value;
            }
        }
    }
}
