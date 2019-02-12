using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace D3DevBlog.WPF.Shared
{
    public static class PropertyHelper
    {
        public static PropertyInfo GetPropertyByPath(this object obj, string propertyPath)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            return GetPropertyByPath(obj.GetType(), propertyPath);
        }

        public static PropertyInfo GetPropertyByPath<T>(string propertyPath) where T : class
        {
            return GetPropertyByPath(typeof(T), propertyPath);
        }

        public static PropertyInfo GetPropertyByPath(this Type type, string propertyPath)
        {
            try
            {
                if (string.IsNullOrEmpty(propertyPath))
                    return null;
                string[] Splitter = { "." };
                string[] SourceProperties = propertyPath.Split(Splitter, StringSplitOptions.None);
                Type PropertyType = type;
                PropertyInfo PropertyInfo = PropertyType.GetProperty(SourceProperties[0]);
                PropertyType = PropertyInfo.PropertyType;
                for (int x = 1; x < SourceProperties.Length; ++x)
                {
                    PropertyInfo = PropertyType.GetProperty(SourceProperties[x]);
                    PropertyType = PropertyInfo.PropertyType;
                }
                return PropertyInfo;
            }
            catch
            {
                throw;
            }
        }
    }
}
