// Copyright (c) Andrew Arnott. All rights reserved.

namespace Spss
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Threading;

    internal static class EmbeddedResources
    {
        public static string LoadFileFromAssemblyWithNamespace(string filename, string namespacePrefix)
        {
            return LoadFileFromAssemblyWithNamespace(filename, namespacePrefix, Assembly.GetCallingAssembly());
        }

        public static string ManifestNameFromFileNameAndNamespace(string filename, string namespacePrefix)
        {
            if ((filename == null) || (filename.Length == 0))
            {
                throw new ArgumentNullException("filename");
            }

            if (namespacePrefix == null)
            {
                namespacePrefix = string.Empty;
            }

            if (filename.Substring(0, 1) != "/")
            {
                filename = "/" + filename;
            }

            string manifestName = filename.Replace('/', '.');
            int pathEndsAt = filename.LastIndexOf('/');
            manifestName = manifestName.Substring(0, pathEndsAt).Replace(' ', '_') + manifestName.Substring(pathEndsAt);
            return namespacePrefix + manifestName;
        }

        public static CultureInfo GetCultureFromManifestName(string manifestName, out string newManifestName)
        {
            newManifestName = manifestName;
            CultureInfo defaultCulture = null;
            Match m = Regex.Match(manifestName, @"\A(?<pre>.+)\.(?<culture>[a-z]{2}(?:-[A-Z]{2})?)(?<post>\.[^\.]+)\z");
            if (!m.Success)
            {
                return defaultCulture;
            }

            try
            {
                CultureInfo culture = CultureInfo.GetCultureInfo(m.Groups["culture"].Value);
                newManifestName = m.Groups["pre"].Value + m.Groups["post"].Value;
                return culture;
            }
            catch (ArgumentException)
            {
                return defaultCulture;
            }
        }

        public static Stream GetLocalizedManifestResourceStream(string fileName, string namespacePrefix, Assembly baseAssembly)
        {
            string manifestName = ManifestNameFromFileNameAndNamespace(fileName, namespacePrefix);
            CultureInfo culture = GetCultureFromManifestName(manifestName, out manifestName);
            if (culture == null)
            {
                return GetLocalizedManifestResourceStream(manifestName, baseAssembly);
            }

            return GetLocalizedManifestResourceStream(manifestName, baseAssembly, culture);
        }

        public static string LoadFileFromAssemblyWithNamespace(string filename, string namespacePrefix, Assembly assembly)
        {
            string str;
            if (filename == null)
            {
                throw new ArgumentNullException("filename");
            }

            if (assembly == null)
            {
                assembly = Assembly.GetCallingAssembly();
            }

            StreamReader reader = new StreamReader(GetLocalizedManifestResourceStream(filename, namespacePrefix, assembly));
            try
            {
                str = reader.ReadToEnd();
            }
            finally
            {
                reader.Close();
            }

            return str;
        }

        public static Stream GetLocalizedManifestResourceStream(string manifestName, Assembly baseAssembly)
        {
            if ((manifestName == null) || (manifestName.Length == 0))
            {
                throw new ArgumentNullException("manifestName");
            }

            if (baseAssembly == null)
            {
                throw new ArgumentNullException("baseAssembly");
            }

            Stream stream = null;
            CultureInfo culture = Thread.CurrentThread.CurrentUICulture;
            stream = GetLocalizedManifestResourceStreamIfExists(manifestName, baseAssembly, culture);
            if (!((stream != null) || culture.IsNeutralCulture))
            {
                stream = GetLocalizedManifestResourceStreamIfExists(manifestName, baseAssembly, culture.Parent);
            }

            if (stream == null)
            {
                stream = GetFileStreamFromAssembly(manifestName, baseAssembly);
            }

            return stream;
        }

        public static Stream GetLocalizedManifestResourceStreamIfExists(string manifestName, Assembly baseAssembly, CultureInfo culture)
        {
            if ((manifestName == null) || (manifestName.Length == 0))
            {
                throw new ArgumentNullException("manifestName");
            }

            if (baseAssembly == null)
            {
                throw new ArgumentNullException("baseAssembly");
            }

            if (culture == null)
            {
                throw new ArgumentNullException("culture");
            }

            try
            {
                return baseAssembly.GetSatelliteAssembly(culture).GetManifestResourceStream(manifestName);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        public static Stream GetFileStreamFromAssembly(string manifestName, Assembly assembly)
        {
            if (manifestName == null)
            {
                throw new ArgumentNullException("manifestName");
            }

            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            Stream fileStream = assembly.GetManifestResourceStream(manifestName);
            if (fileStream == null)
            {
                throw new ArgumentOutOfRangeException("manifestName", manifestName, "The embedded file could not be found in the assembly " + assembly.FullName + ".  Check to see that the file's Build Action attribute in the project is set to \"Embedded Resource\".");
            }

            return fileStream;
        }

        public static Stream GetLocalizedManifestResourceStream(string manifestName, Assembly baseAssembly, CultureInfo culture)
        {
            if ((manifestName == null) || (manifestName.Length == 0))
            {
                throw new ArgumentNullException("manifestName");
            }

            if (baseAssembly == null)
            {
                throw new ArgumentNullException("baseAssembly");
            }

            if (culture == null)
            {
                throw new ArgumentNullException("culture");
            }

            Stream stream = baseAssembly.GetSatelliteAssembly(culture).GetManifestResourceStream(manifestName);
            if ((stream == null) && manifestName.Contains("_"))
            {
                stream = baseAssembly.GetSatelliteAssembly(culture).GetManifestResourceStream(manifestName.Replace('_', ' '));
            }

            return stream;
        }
    }
}
