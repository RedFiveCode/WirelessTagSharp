using System.IO;
using System.Reflection;
using System;
using System.IO.Abstractions;

namespace WirelessTagClientApp
{
    public class OptionsExtensions
    {
        private IFileSystem _fileSystem;

        public OptionsExtensions()
        {
            _fileSystem = new FileSystem();
        }

        public OptionsExtensions(IFileSystem fileSystem = null)
        {
            _fileSystem = fileSystem ?? new FileSystem();
        }

        /// <summary>
        /// Returns cache folder name, resolving relative path to a rooted path
        /// </summary>
        public string ResolveCacheFolder(Options options)
        {
            if (options == null || String.IsNullOrEmpty(options.CacheFolder))
            {
                return null;
            }

            if (Path.IsPathRooted(options.CacheFolder))
            {
                // assume a rooted path exists
                return options.CacheFolder;
            }

            // resolve relative path to absolute path
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyPath = Path.GetDirectoryName(assembly.Location);

            // convert relative path to absolute path
            var cachePath = Path.Combine(assemblyPath, options.CacheFolder);
            var cacheFolder = Path.GetFullPath(cachePath);

            return _fileSystem.Directory.Exists(cacheFolder) ? cacheFolder : null;
        }
    }
}
