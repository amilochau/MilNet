using System;
using System.Globalization;
using System.Linq;

namespace MilNet.Core.Types
{
    /// <summary>Types extensions</summary>
    public static class TypesExtensions
    {
        #region DateTime

        /// <summary>Check if a string can be parsed to a DateTime</summary>
        /// <param name="value">Value</param>
        /// <param name="styles">Styles</param>
        /// <param name="provider">Provider</param>
        public static bool CheckDateTime(this string value, DateTimeStyles styles, IFormatProvider provider)
        {
            return (!string.IsNullOrEmpty(value) && DateTime.TryParse(value, provider, styles, out DateTime d));
        }
        /// <summary>Check a string and convert it to DateTime</summary>
        /// <param name="value">Value</param>
        /// <param name="styles">Styles</param>
        /// <param name="provider">Provider</param>
        /// <exception cref="FormatException">Not a DateTime</exception>
        public static DateTime ConvertDateTime(this string value, DateTimeStyles styles, IFormatProvider provider)
        {
            if (!string.IsNullOrEmpty(value) && DateTime.TryParse(value, provider, styles, out DateTime d))
                return d;
            else
                throw new FormatException();
        }
        /// <summary>Check a string and convert it to DateTime</summary>
        /// <param name="value">Value</param>
        /// <param name="styles">Styles</param>
        /// <param name="provider">Provider</param>
        /// <returns>Conversion to DateTime, or null</returns>
        public static DateTime? ConvertDateTimeSafe(this string value, DateTimeStyles styles, IFormatProvider provider)
        {
            if (!string.IsNullOrEmpty(value) && DateTime.TryParse(value, provider, styles, out DateTime d))
                return d;
            else
                return null;
        }

        #endregion
        #region MySql DateTime

        /// <summary>Check if a string can be parsed to a DateTime</summary>
        /// <param name="value">Value</param>
        /// <param name="styles">Styles</param>
        /// <param name="provider">Provider</param>
        /// <remarks>The year must be upper than 1753 (MySql restriction)</remarks>
        public static bool CheckMySqlDateTime(this string value, DateTimeStyles styles, IFormatProvider provider)
        {
            return (!string.IsNullOrEmpty(value) && DateTime.TryParse(value, provider, styles, out DateTime d) && d.Year >= 1753); // See System.Data.SqlTypes.SqlDateTime.MinValue
        }
        /// <summary>Check a string and convert it to DateTime</summary>
        /// <param name="value">Value</param>
        /// <param name="styles">Styles</param>
        /// <param name="provider">Provider</param>
        /// <remarks>The year must be upper than 1753 (MySql restriction)</remarks>
        /// <exception cref="FormatException">Not a DateTime</exception>
        public static DateTime ConvertMySqlDateTime(this string value, DateTimeStyles styles, IFormatProvider provider)
        {
            if (!string.IsNullOrEmpty(value) && DateTime.TryParse(value, provider, styles, out DateTime d) && d.Year >= 1753) // See System.Data.SqlTypes.SqlDateTime.MinValue
                return d;
            else
                throw new FormatException();
        }
        /// <summary>Check a string and convert it to DateTime</summary>
        /// <param name="value">Value</param>
        /// <param name="styles">Styles</param>
        /// <param name="provider">Provider</param>
        /// <remarks>The year must be upper than 1753 (MySql restriction)</remarks>
        /// <returns>Conversion to DateTime, or null</returns>
        public static DateTime? ConvertMySqlDateTimeSafe(this string value, DateTimeStyles styles, IFormatProvider provider)
        {
            if (!string.IsNullOrEmpty(value) && DateTime.TryParse(value, provider, styles, out DateTime d) && d.Year >= 1753) // See System.Data.SqlTypes.SqlDateTime.MinValue
                return d;
            else
                return null;
        }

        #endregion
        #region Double

        /// <summary>Check if a string can be parsed to a double</summary>
        /// <param name="value">Value</param>
        /// <param name="style">Number styles</param>
        /// <param name="provider">Format provider</param>
        /// <exception cref="ArgumentNullException"/>
        public static bool CheckDouble(this string value, NumberStyles style, CultureInfo provider)
        {
            value = value.ReplaceNumberDecimalSeparator(provider);
            return (!string.IsNullOrEmpty(value) && double.TryParse(value, style, provider, out double d));
        }
        /// <summary>Check a string and convert it to double</summary>
        /// <param name="value">Value</param>
        /// <param name="style">Number styles</param>
        /// <param name="provider">Format provider</param>
        /// <exception cref="FormatException">Not a double</exception>
        /// <exception cref="ArgumentNullException"/>
        public static double ConvertDouble(this string value, NumberStyles style, CultureInfo provider)
        {
            value = value.ReplaceNumberDecimalSeparator(provider);
            if (!string.IsNullOrEmpty(value) && double.TryParse(value, style, provider, out double d))
                return d;
            else
                throw new FormatException();
        }
        /// <summary>Check a string and convert it to double</summary>
        /// <param name="value">Value</param>
        /// <param name="style">Number styles</param>
        /// <param name="provider">Format provider</param>
        /// <exception cref="FormatException">Not a double</exception>
        /// <returns>Conversion to double, or null</returns>
        public static double? ConvertDoubleSafe(this string value, NumberStyles style, CultureInfo provider)
        {
            value = value.ReplaceNumberDecimalSeparator(provider);
            if (!string.IsNullOrEmpty(value) && double.TryParse(value, style, provider, out double d))
                return d;
            else
                return null;
        }

        #endregion

        /// <summary>Replace point and comma by the number decimal separator of a culture</summary>
        /// <param name="value">Value</param>
        /// <param name="provider">Format provider</param>
        /// <exception cref="ArgumentNullException"/>
        public static string ReplaceNumberDecimalSeparator(this string value, CultureInfo provider)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            char separator = provider.NumberFormat.NumberDecimalSeparator.First();
            value = value.Replace('.', separator);
            value = value.Replace(',', separator);
            return value;
        }
    }
}
