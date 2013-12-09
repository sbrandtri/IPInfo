using System.Collections.Generic;

namespace IPInfo.Configuration
{
    /// <summary>
    /// An IComparer for sorting provider configurations by priority.
    /// </summary>
    class ProviderPriorityComparer : IComparer<ProviderElement>
    {
        public int Compare(ProviderElement x, ProviderElement y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're equal.  
                    return 0;
                }
                
                // If x is null and y is not null, y is greater.  
                return -1;
            }
            
            // If x is not null... 

            if (y == null)
            {
                // ...and y is null, x is greater.
                return 1;
            }
            
            // ...and y is not null, compare the priority values. 
            return x.Priority.CompareTo(y.Priority);
        }
    }
}
