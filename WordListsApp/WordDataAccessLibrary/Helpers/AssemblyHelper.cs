using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WordDataAccessLibrary.CollectionBackupServices.JsonServices;

namespace WordDataAccessLibrary.Helpers;
public static class AssemblyHelper
{
    public static class CurrentAssembly
    {
        /// <summary>
        /// Version in format X.X.X.X where X is number
        /// </summary>
        public static string Version { get; } = GetAssemblyVersion();

        /// <summary>
        /// Version in format vX.X.X where X is number, if fails returns v0.0.0
        /// </summary>
        public static string VersionString { get; } = GetAssemblyVersionString();
        public static string Name { get; } = GetAssemblyName();

        private static string GetAssemblyVersionString()
        {
            string version = GetAssemblyVersion();

            string[] vNumbers = version.Split('.');

            if (vNumbers.Length >= 3)
            {
                return $"v{vNumbers[0]}.{vNumbers[1]}.{vNumbers[2]}";
            }
            return "v0.0.0";

        }
        private static string GetAssemblyVersion()
        {
            return typeof(AssemblyHelper).Assembly.GetName().Version.ToString();
        }
        private static string GetAssemblyName()
        {
            return typeof(AssemblyHelper).Assembly.GetName().Name;
        }

    }

    public static class EntryAssembly
    {
        /// <summary>
        /// Version in format X.X.X.X where X is number
        /// </summary>
        public static string Version { get; } = GetAssemblyVersion();

        public static string Name { get; } = GetAssemblyName();

   

        /// <summary>
        /// Version in format vX.X.X where X is number, if fails returns v0.0.0
        /// </summary>
        public static string VersionString { get; } = GetAssemblyVersionString();
        private static string GetAssemblyVersionString()
        {
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            int[] vNumbers =
            {
                version.Major,
                version.Minor,
                version.Build,
            };
            return $"v{vNumbers[0]}.{vNumbers[1]}.{vNumbers[2]}";
        }
        private static string GetAssemblyVersion()
        {
            return Assembly.GetEntryAssembly().GetName().Version.ToString();
        }
        private static string GetAssemblyName()
        {
            return Assembly.GetEntryAssembly().GetName().Name;
        }
    }
}
