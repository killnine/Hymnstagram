using Hymnstagram.Model.DataAccess.PropertyMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Hymnstagram.Web.Helpers.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> ApplySort<T>(this IEnumerable<T> source, string orderBy, Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if(source == null) 
            { 
                throw new ArgumentNullException(nameof(source)); 
            }            

            if(mappingDictionary == null) 
            { 
                throw new ArgumentNullException(nameof(mappingDictionary)); 
            }

            if(string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            //orderBy is separated by comma, so split it.
            var orderByAfterSplit = orderBy.Split(',');

            //apply each orderBy clause in reverse order
            var sourceQueryable = source.AsQueryable<T>();
            foreach (var orderByClause in orderByAfterSplit.Reverse())
            {
                //can't trim in foreach
                var trimmedOrderByClause = orderByClause.Trim();

                //if order ends ith " desc", order descending, otherwise ascending
                var orderDescending = trimmedOrderByClause.EndsWith(" desc");

                //remove " asc" or " desc" from the orderByClause to get property to use
                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ? trimmedOrderByClause : trimmedOrderByClause.Remove(indexOfFirstSpace);

                //find the matching property
                if (!mappingDictionary.ContainsKey(propertyName))
                {
                    throw new ArgumentException($"Key mapping for {propertyName} is missing.");
                }

                //get the PropertyMappingValue
                var propertyMappingValue = mappingDictionary[propertyName];

                if (propertyMappingValue == null)
                {
                    throw new ArgumentNullException("propertyMappingValue");
                }

                //Run through property names in reverse so orderBy clause is applied correctly                
                foreach (var destinationProperty in propertyMappingValue.DestinationProperties.Reverse())
                {
                    //reverse sort order if necessary
                    if (propertyMappingValue.Reverse)
                    {
                        orderDescending = !orderDescending;
                    }
                    sourceQueryable = sourceQueryable.OrderBy(destinationProperty + (orderDescending ? " descending" : " ascending"));
                }
            }
            return sourceQueryable;
        }
    }
}
