using System.Collections.Generic;

namespace Hymnstagram.Model.DataAccess.PropertyMapping
{
    public class PropertyMappingValue
    {
        public IEnumerable<string> DestinationProperties { get; private set; }
        public bool Reverse { get; private set; }
        public PropertyMappingValue(IEnumerable<string> destinationProperties, bool reverse = false)
        {
            DestinationProperties = destinationProperties;
            Reverse = reverse;
        }
    }
}
