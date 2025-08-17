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
    public class CacheFileReaderWriter
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

        /// <summary>
        /// Gets the name of the cache file for a specific tag in the specified folder.
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="folder"/> is null or empty</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="tag"/> is null</exception>
        public string GetCacheFilename(string folder, TagInfo tag)
        {
            if (string.IsNullOrWhiteSpace(folder))
            {
                throw new ArgumentNullException(nameof(folder), "Folder cannot be null or empty");
            }

            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag), "Tag cannot be null");
            }

            // Create a filename based on the tag's UUID and the date range
            var filename = $"{tag.Uuid}.cache.json.gz";

            return Path.Combine(folder, filename);
        }

        /// <summary>
        /// Writes the cache file for a specific tag with the provided data.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="data"></param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="filename"/> is null or empty</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="data"/> is null</exception>
        public void WriteCacheFile(string filename, List<Measurement> data)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentNullException(nameof(filename), "Filename cannot be null or empty");
            }

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), "Data cannot be null");
            }

            var serializer = new JsonSerializer()
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                Formatting = Formatting.Indented
            };

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

        /// <summary>
        /// Reads the cache file for a specific tag.
        /// </summary>
        /// <remarks>
        /// Returns an empty list if the file does not exist or is empty.
        /// </remarks>
        /// <param name="filename"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="filename"/> is null or empty</exception>
        public List<Measurement> ReadCacheFile(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentNullException(nameof(filename), "Filename cannot be null or empty");
            }

            if (!_fileSystem.File.Exists(filename))
            {
                return new List<Measurement>(); // empty list
            }


            var serializer = new JsonSerializer
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                Formatting = Formatting.Indented,
            };

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

    }
}
