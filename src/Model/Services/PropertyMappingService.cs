using Hymnstagram.Model.DataAccess.PropertyMapping;
using Hymnstagram.Model.DataTransfer;
using Hymnstagram.Model.Domain;
using Hymnstagram.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hymnstagram.Model.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _songbookPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>() { "Id"}) },
                {"Title", new PropertyMappingValue(new List<string>() { "Title" }) },
                {"Publisher", new PropertyMappingValue(new List<string>() { "Publisher" }) },
                {"ISBN10", new PropertyMappingValue(new List<string>() { "ISBN10" }) },
                {"ISBN13", new PropertyMappingValue(new List<string>() { "ISBN13" }) }
            };

        private IList<IPropertyMapping> propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            propertyMappings.Add(new PropertyMapping<SongbookDto, Songbook>(_songbookPropertyMapping));
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            var matchingMapping = propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }

            throw new Exception($"Cannot find exact property mapping instance for <{typeof(TSource)}, {typeof(TDestination)}>");
        }

    }
}
