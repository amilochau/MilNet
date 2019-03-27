using MilNet.Core.Types;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace MilNet.Core.Attributes
{
    /// <summary>Display data table properties</summary>
    public static class DataTableDisplay
    {
        /// <summary>Get headers of class containing <see cref="DataTableAttribute"/> indicators</summary>
        /// <param name="type">Type</param>
        /// <exception cref="ArgumentNullException"/>
        public static Collection<string> GetDataTableHeaders(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            Collection<string> headers = new Collection<string>();
            foreach (PropertyInfo property in type.GetProperties())
            {
                object[] customAttributes = property.GetCustomAttributes(true);
                foreach (object customAttribute in customAttributes)
                {
                    DataTableAttribute dataTableAttribute = customAttribute as DataTableAttribute;
                    if (dataTableAttribute != null)
                    {
                        headers.Add(dataTableAttribute.Header ?? property.Name);
                        break;
                    }
                }
            }
            return headers;
        }
        /// <summary>Get styles of class containing <see cref="DataTableAttribute"/> indicators</summary>
        /// <param name="type">Type</param>
        /// <exception cref="ArgumentNullException"/>
        public static Collection<string> GetDataTableStyles(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            Collection<string> styles = new Collection<string>();
            foreach (PropertyInfo property in type.GetProperties())
            {
                object[] customAttributes = property.GetCustomAttributes(true);
                foreach (object customAttribute in customAttributes)
                {
                    DataTableAttribute dataTableAttribute = customAttribute as DataTableAttribute;
                    if (dataTableAttribute != null)
                    {
                        styles.Add(dataTableAttribute.Format);
                        break;
                    }
                }
            }
            return styles;
        }

        /// <summary>Get properties of class containing <see cref="DataTableAttribute"/> indicators</summary>
        /// <param name="type">Type</param>
        /// <exception cref="ArgumentNullException"/>
        public static Collection<PropertyInfo> GetDataTableProperties(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            DataCollection<PropertyInfo> attributes = new DataCollection<PropertyInfo>();
            attributes.AddRange(type.GetProperties().Where(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(DataTableAttribute))));
            return attributes;
        }

        /// <summary>Get formatted values class properties containing <see cref="DataTableAttribute"/> indicators</summary>
        /// <param name="instance">Instance</param>
        public static Collection<object> GetDataTableValues<TData>(this TData instance) where TData : class
        {
            Collection<object> values = new Collection<object>();
            foreach (PropertyInfo property in typeof(TData).GetProperties())
            {
                object[] customAttributes = property.GetCustomAttributes(true);
                foreach (object customAttribute in customAttributes)
                {
                    DataTableAttribute dataTableAttribute = customAttribute as DataTableAttribute;
                    if (dataTableAttribute != null)
                    {
                        values.Add(property.GetValue(instance));
                        break;
                    }
                }
            }
            return values;
        }

        /// <summary>Get formatted values class properties containing <see cref="DataTableAttribute"/> indicators</summary>
        /// <param name="instance">Instance</param>
        /// <param name="formatProvider">Format provider</param>
        public static Collection<string> GetDataTableFormattedValues<TData>(this TData instance, IFormatProvider formatProvider) where TData : class
        {
            Collection<string> values = new Collection<string>();
            foreach (PropertyInfo property in typeof(TData).GetProperties())
            {
                object[] customAttributes = property.GetCustomAttributes(true);
                foreach (object customAttribute in customAttributes)
                {
                    DataTableAttribute dataTableAttribute = customAttribute as DataTableAttribute;
                    if (dataTableAttribute != null)
                    {
                        var value = property.GetValue(instance);
                        if (value == null)
                            values.Add("");
                        else
                        {
                            string style = dataTableAttribute.Format;
                            if (string.IsNullOrEmpty(style))
                                values.Add(value.ToString());
                            else
                            {
                                IFormattable formattableValue = value as IFormattable;
                                if (formattableValue == null)
                                    values.Add(value.ToString());
                                else
                                    values.Add(formattableValue.ToString(style, formatProvider));
                            }
                        }
                        break;
                    }
                }
            }
            return values;
        }
    }
}
