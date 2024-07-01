using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Generics
{
	public class ObjectValidation
	{
        public static bool IsAnyNullOrEmpty(object myObject)
        {
            foreach(PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if(pi.PropertyType == typeof(string))
                {
                    string value = (string)pi.GetValue(myObject);
                    if(string.IsNullOrEmpty(value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsAllNullOrEmptyOrZero(object myObject)
        {
            foreach(PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if(pi.PropertyType == typeof(string))
                {
                    string value = (string)pi.GetValue(myObject);
                    if(!string.IsNullOrEmpty(value))
                    {
                        return false;
                    }
                }
                else if(pi.GetValue(myObject) != null &&(pi.PropertyType == typeof(int) || (pi.PropertyType.GetProperties().Any(t => t.Name == "Value") && pi.PropertyType.GetProperties().FirstOrDefault(t => t.Name == "Value").PropertyType == typeof(int))))
                {
                    int value = (int)pi.GetValue(myObject);
                    if(value != 0)
                    {
                        return false;
                    }
                }
                else if (pi.GetValue(myObject) != null && (pi.PropertyType == typeof(decimal) || (pi.PropertyType.GetProperties().Any(t => t.Name == "Value") && pi.PropertyType.GetProperties().FirstOrDefault(t => t.Name == "Value").PropertyType == typeof(decimal))))
                {
                    decimal value = (decimal)pi.GetValue(myObject);
                    if (value != 0)
                    {
                        return false;
                    }
                }
                else if(pi.GetValue(myObject)!=null && (pi.PropertyType == typeof(DateTime) || (pi.PropertyType.GetProperties().Any(t => t.Name == "Value") && pi.PropertyType.GetProperties().FirstOrDefault(t => t.Name == "Value").PropertyType == typeof(DateTime))))
                {
                    
                    DateTime value = (DateTime)pi.GetValue(myObject);
                    if(value != DateTime.MinValue)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
