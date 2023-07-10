using System.Collections.Generic;
using System.Reflection;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public static class CoreExtensions
    {
        private static readonly Dictionary<string, PropertyInfo[]> browsablePropertyInfos =
            new Dictionary<string, PropertyInfo[]>();

        public static PropertyInfo[] GetBrowsableProperties(this object obj)
        {
            var key = obj.GetType().ToString();

            if (!browsablePropertyInfos.ContainsKey(key))
            {
                var propertyInfoList = new List<PropertyInfo>();
                var properties = obj.GetType().GetProperties();
                foreach (var property in properties)
                {
                    if ((property.PropertyType.IsSubclassOf(typeof (IEntity)) ||
                         property.PropertyType.GetInterface("IList") != null))
                    {
                        propertyInfoList.Add(property);
                    }
                }

                browsablePropertyInfos.Add(key, propertyInfoList.ToArray());
            }

            return browsablePropertyInfos[key];
        }
    }
}