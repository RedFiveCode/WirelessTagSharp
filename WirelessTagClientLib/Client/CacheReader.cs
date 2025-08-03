using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientLib.Client
{
    /// <summary>
    /// CacheReader is responsible for reading cached temperature measurement data for a specific tag from a cache file/folder
    /// </summary>
    public class CacheReader
    {
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheReader"/> class
        /// </summary>
        public CacheReader() : this (new FileSystem())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheReader"/> class for unit testing
        /// </summary>
        /// <param name="fileSystem"></param>
        public CacheReader(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        /// <summary>
        /// Reads the cache for a specific tag from the specified folder.
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public List<Measurement> ReadCache(string folder, TagInfo tag)
        {
            if (string.IsNullOrWhiteSpace(folder))
            {
                throw new ArgumentNullException(nameof(folder), "Folder cannot be null or empty");
            }
            ThrowIf.Argument.IsNull(tag, nameof(tag));

            if (!_fileSystem.Directory.Exists(folder))
            {
                throw new DirectoryNotFoundException($"Cache folder '{folder}' does not exist.");
            }

            var filename = GetCacheFilename(folder, tag);

            if (!_fileSystem.File.Exists(filename))
            {
                return new List<Measurement>(); // empty list
            }

            var json = _fileSystem.File.ReadAllText(filename);

            var settings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                Formatting = Formatting.Indented
            };

            return JsonConvert.DeserializeObject<List<Measurement>>(json, settings);
        }

        private string GetCacheFilename(string folder, TagInfo tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag), "Tag cannot be null");
            }

            // Create a filename based on the tag's UUID and the date range
            var filename = $"{tag.Uuid}.cache.json";

            return Path.Combine(folder, filename);
        }
    }
}
