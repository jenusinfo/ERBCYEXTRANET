using CMS.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Eurobank.Services
{
    public class DisplayNameLocalizedAttribute : DisplayNameAttribute
    {
        public DisplayNameLocalizedAttribute(string name) : base(name) { }
        public DisplayNameLocalizedAttribute(string resourceKey, Type resourceType)
            : base(GetDisplayName(resourceKey, resourceType)) { }

        public static string GetDisplayName(string resourceKey, Type resourceType)
        {
            PropertyInfo property = resourceType.GetProperty(resourceKey,
                BindingFlags.Public | BindingFlags.Static);
            return ResHelper.GetString(resourceKey);
            // return (string)property.GetValue(property.DeclaringType, null);
        }
    }
}
