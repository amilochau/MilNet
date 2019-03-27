using System;
using System.Collections.Generic;
using System.Linq;

namespace MilNet.Core.Configuration
{
    /// <summary>Cookie for theme management</summary>
    [System.Obsolete("Will be removed in version 3. Please use standard .NET Core features instead.")]
    public class ThemeConfiguration
    {
        /// <summary>Valid themes</summary>
        private static readonly List<string> validThemes = new List<string> { "light", "dark" };
        /// <summary>Implemented themes</summary>
        private List<string> implementedThemes = new List<string>() { "light" };

        /// <summary>Constructor</summary>
        /// <param name="implementedThemes">Implemented themes</param>
        /// <exception cref="ArgumentNullException"/>
        public ThemeConfiguration(params string[] implementedThemes)
        {
            if (implementedThemes == null)
                throw new ArgumentNullException(nameof(implementedThemes));

            foreach (string theme in implementedThemes)
                this.implementedThemes.Add(theme);
        }

        /// <summary>Search an implemented theme</summary>
        /// <param name="name">Theme name ("light" by example)</param>
        /// <returns>Correct theme; or "light" by default</returns>
        public string GetImplementedTheme(string name)
        {
            if (string.IsNullOrEmpty(name))
                return DefaultTheme;

            // Valid theme?
            if (validThemes.Where(c => c.Equals(name, StringComparison.OrdinalIgnoreCase)).Count() == 0)
                return DefaultTheme;

            // Implemented theme?
            if (implementedThemes.Where(c => c.Equals(name, StringComparison.OrdinalIgnoreCase)).Count() > 0)
                return name;

            // If theme is not implemented: default theme
            return DefaultTheme;
        }

        /// <summary>Default theme ("light")</summary>
        public string DefaultTheme => implementedThemes[0];
    }
}
