using System.Reflection;

#nullable enable

namespace WordDataAccessLibrary.Helpers;
public static class AssemblyHelper
{
    public static class CurrentAssembly
    {
        /// <summary>
        /// Version in format X.X.X.X where X is number, might return null
        /// </summary>
        public static string? Version { get; } = GetAssemblyVersion();

        /// <summary>
        /// Version in format vX.X.X where X is number, might return null
        /// </summary>
        public static string? VersionString { get; } = GetAssemblyVersionString();
        public static string? Name { get; } = GetAssemblyName();

        private static string? GetAssemblyVersionString()
        {
            Version? version = typeof(CurrentAssembly).Assembly.GetName().Version;
            if (version is null) return null;
            return $"v{version.Major}.{version.Minor}.{version.MajorRevision}";
        }
        private static string? GetAssemblyVersion()
        {
            return typeof(AssemblyHelper).Assembly.GetName().Version?.ToString();
        }
        private static string? GetAssemblyName()
        {
            return typeof(AssemblyHelper).Assembly.GetName().Name;
        }
    }

    public static class EntryAssembly
    {
        /// <summary>
        /// Version in format X.X.X.X where X is number, might return null
        /// </summary>
        public static string? Version { get; } = GetAssemblyVersion();

        /// <summary>
        /// Version in format vX.X.X where X is number, might return null
        /// </summary>
        public static string? VersionString { get; } = GetAssemblyVersionString();
        public static string? Name { get; } = GetAssemblyName();

        private static string? GetAssemblyVersionString()
        {
            Version? version = Assembly.GetEntryAssembly()?.GetName().Version;
            if (version is null) return null;
            return $"v{version.Major}.{version.Minor}.{version.MajorRevision}";
        }
        private static string? GetAssemblyVersion()
        {
            string? version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString();
            if (version is null) return null;
            return version;
        }
        private static string? GetAssemblyName()
        {
            return Assembly.GetEntryAssembly()?.GetName().Name;
        }
    }
}
