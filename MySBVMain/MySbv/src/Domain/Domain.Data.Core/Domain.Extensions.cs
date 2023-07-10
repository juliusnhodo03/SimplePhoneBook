﻿using System.Collections.Generic;
using System.Reflection;

namespace Domain.Data.Core
{
    public static class DomainExtensions
    {
        private static readonly Dictionary<string, PropertyInfo[]> browsablePropertyInfos =
            new Dictionary<string, PropertyInfo[]>();

        public static PropertyInfo[] GetBrowsableProperties(this object obj)
        {
            string key = obj.GetType().ToString();

            if (!browsablePropertyInfos.ContainsKey(key))
            {
                var propertyInfoList = new List<PropertyInfo>();
                PropertyInfo[] properties = obj.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
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