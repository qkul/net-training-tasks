using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Reflection.Tasks
{
    public static class CommonTasks
    {

        /// <summary>
        /// Returns the lists of public and obsolete classes for specified assembly.
        /// Please take attention: classes (not interfaces, not structs)
        /// </summary>
        /// <param name="assemblyName">name of assembly</param>
        /// <returns>List of public but obsolete classes</returns>
        public static IEnumerable<string> GetPublicObsoleteClasses(string assemblyName) {
            // TODO : Implement GetPublicObsoleteClasses method
            // throw new NotImplementedException();
            var assembly = Assembly.ReflectionOnlyLoad(assemblyName);
            return assembly.GetExportedTypes().
                Where(x => x.IsClass && x.GetCustomAttributes(typeof(ObsoleteAttribute), true).Any()).
                Select(x => x.Name);

        }

        /// <summary>
        /// Returns the value for required property path
        /// </summary>
        /// <example>
        ///  1) 
        ///  string value = instance.GetPropertyValue("Property1")
        ///  The result should be equal to invoking statically
        ///  string value = instance.Property1;
        ///  2) 
        ///  string name = instance.GetPropertyValue("Property1.Property2.FirstName")
        ///  The result should be equal to invoking statically
        ///  string name = instance.Property1.Property2.FirstName;
        /// </example>
        /// <typeparam name="T">property type</typeparam>
        /// <param name="obj">source object to get property from</param>
        /// <param name="propertyPath">dot-separated property path</param>
        /// <returns>property value of obj for required propertyPath</returns>
        public static T GetPropertyValue<T>(this object obj, string propertyPath) {
            // TODO : Implement GetPropertyValue method
            //throw new NotImplementedException();
            var properties = propertyPath.Split('.');
            foreach (var prop in properties)
            {
                obj = obj.GetType().GetProperty(prop).GetValue(obj, null);
            }
            return (T)obj;
        }


        /// <summary>
        /// Assign the value to the required property path
        /// </summary>
        /// <example>
        ///  1)
        ///  instance.SetPropertyValue("Property1", value);
        ///  The result should be equal to invoking statically
        ///  instance.Property1 = value;
        ///  2)
        ///  instance.SetPropertyValue("Property1.Property2.FirstName", value);
        ///  The result should be equal to invoking statically
        ///  instance.Property1.Property2.FirstName = value;
        /// </example>
        /// <param name="obj">source object to set property to</param>
        /// <param name="propertyPath">dot-separated property path</param>
        /// <param name="value">assigned value</param>
        public static void SetPropertyValue(this object obj, string propertyPath, object value) {
            // TODO : Implement SetPropertyValue method
            var properties = propertyPath.Split('.');
            var newObj = properties.Length == 1 ? obj :
                GetPropertyValue<object>(obj, String.Join(".", properties.Take(properties.Length - 1)));
            newObj.GetType().
                BaseType.InvokeMember(properties.Last(), BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, newObj, new object[] { value });

        }
    }
}
