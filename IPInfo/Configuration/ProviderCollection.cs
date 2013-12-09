using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using IPInfo.Annotations;

namespace IPInfo.Configuration
{
    /// <summary>
    /// A collection of provider configurations
    /// </summary>
    [UsedImplicitly]
    class ProviderCollection : ConfigurationElementCollection, IEnumerable<ProviderElement>
    {
        private const string ProviderElementName = "provider";

        protected override ConfigurationElement CreateNewElement()
        {
            return new ProviderElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ProviderElement)element).ProviderName;
        }

        protected override string ElementName
        {
            get { return ProviderElementName; }
        }

        protected override bool IsElementName(string elementName)
        {
            return !String.IsNullOrEmpty(elementName) && elementName == ProviderElementName;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        [UsedImplicitly]
        public ProviderElement this[int index]
        {
            get { return BaseGet(index) as ProviderElement; }
        }

        public new IEnumerator<ProviderElement> GetEnumerator()
        {
            IEnumerator ie = base.GetEnumerator();
            while (ie.MoveNext())
            {
                yield return (ProviderElement)ie.Current;
            }
        }
    }
}