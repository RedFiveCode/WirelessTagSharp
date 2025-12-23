using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Compression;
using System.Text;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientLib.Client
{
    public class CacheFileReaderWriter : ICacheFileReaderWriter
    {
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheFileReaderWriter"/> class
        /// </summary>
        public CacheFileReaderWriter() : this(new FileSystem())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheFileReaderWriter"/> class.
        /// </summary>
        /// <param name="fileSystem"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CacheFileReaderWriter(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem), "File system cannot be null");
        }

        /// <inheritdoc/>
        public string GetCacheFilename(string folder, TagInfo tag)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(folder, nameof(folder));
            ArgumentNullException.ThrowIfNull(tag, nameof(tag));

            // Create a filename based on the tag's UUID
            var filename = $"{tag.Uuid}.cache.json.gz";

            return Path.Combine(folder, filename);
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="filename"/> is null or empty</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="data"/> is null</exception>
        public void WriteCacheFile(string filename, List<Measurement> data)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(filename, nameof(filename));
            ArgumentNullException.ThrowIfNull(data, nameof(data));

            var serializer = CreateSeriaizer();

            using (var stream = _fileSystem.FileStream.New(filename, FileMode.Create, FileAccess.Write))
            {
                using (var compressor = new GZipStream(stream, CompressionMode.Compress))
                {
                    using (var tw = new StreamWriter(compressor, Encoding.UTF8))
                    {
                        // Serialize the data to the file
                        using (var writer = new JsonTextWriter(tw))
                        {
                            serializer.Serialize(writer, data);
                        }
                    }
                }
            }
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="filename"/> is null or empty</exception>
        public List<Measurement> ReadCacheFile(string filename)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(filename, nameof(filename));

            if (!_fileSystem.File.Exists(filename))
            {
                return new List<Measurement>(); // empty list
            }

            var serializer = CreateSeriaizer();

            using (var stream = _fileSystem.FileStream.New(filename, FileMode.Open, FileAccess.Read))
            {
                using (var compressor = new GZipStream(stream, CompressionMode.Decompress))
                {
                    using (var tw = new StreamReader(compressor, Encoding.UTF8))
                    {
                        // Serialize the data to the file
                        using (var reader = new JsonTextReader(tw))
                        {
                            return serializer.Deserialize<List<Measurement>>(reader);
                        }
                    }
                }
            }
        }

        private static JsonSerializer CreateSeriaizer()
        {
            return new JsonSerializer
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                Formatting = Formatting.Indented,
            };
        }
    }
}
