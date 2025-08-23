using System.Collections.Generic;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientLib
{
    /// <summary>
    /// Interface to local file cache.
    /// </summary>
    public interface ICacheFileReaderWriter
    {
        /// <summary>
        /// Gets the name of the cache file for a specific tag in the specified folder.
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        string GetCacheFilename(string folder, TagInfo tag);

        /// <summary>
        /// Reads the cache file for a specific tag.
        /// </summary>
        /// <remarks>
        /// Returns an empty list if the file does not exist or is empty.
        /// </remarks>
        /// <param name="filename"></param>
        /// <returns></returns>
        List<Measurement> ReadCacheFile(string filename);

        /// <summary>
        /// Writes the cache file for a specific tag with the provided data.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="data"></param>
        void WriteCacheFile(string filename, List<Measurement> data);
    }
}