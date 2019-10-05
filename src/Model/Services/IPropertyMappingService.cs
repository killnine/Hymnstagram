using Hymnstagram.Model.DataAccess.PropertyMapping;
using System.Collections.Generic;

namespace Hymnstagram.Web.Services
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
        bool ValidMappingExistsFor<TSource, TDestination>(string fields);
    }
}
