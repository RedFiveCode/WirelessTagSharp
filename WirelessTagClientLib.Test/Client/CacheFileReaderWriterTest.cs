using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using WirelessTagClientLib.Client;
using WirelessTagClientLib.DTO;
using Xunit;

namespace WirelessTagClientLib.Test.Client
{
    public class CacheFileReaderWriterTest
    {
        private readonly MockFileSystem _mockfileSystem;
        private readonly CacheFileReaderWriter _sut;

        public CacheFileReaderWriterTest()
        {
            _mockfileSystem = new MockFileSystem();
            _mockfileSystem.AddDirectory(@"C:\root");
            _sut = new CacheFileReaderWriter(_mockfileSystem);
        }

        [Fact]
        public void Class_Implements_Interface()
        {
            // act
            Assert.IsAssignableFrom<ICacheFileReaderWriter>(_sut);
        }

        [Fact]
        public void GetCacheFilename_FolderNull_ThrowsArgumentNullException()
        {
            // arrange
            var tag = new TagInfo() { SlaveId = 1, Name = "TestTag", Uuid = Guid.Parse("11111111-1111-1111-1111-111111111111") };

            // act; should throw
            Assert.Throws<ArgumentNullException>(() => _sut.GetCacheFilename(null, tag));
        }

        [Fact]
        public void GetCacheFilename_TagNull_ThrowsArgumentNullException()
        {
            // act; should throw
            Assert.Throws<ArgumentNullException>(() => _sut.GetCacheFilename(@"c:\folder", null));
        }

        [Fact]
        public void GetCacheFilename_ReturnsExpectedFilename()
        {
            // arrange
            var tag = new TagInfo() { SlaveId = 1, Name = "TestTag", Uuid = Guid.Parse("11111111-1111-1111-1111-111111111111") };

            // act
            var result = _sut.GetCacheFilename(@"c:\folder", tag);

            // assert
            Assert.Equal(@"c:\folder\11111111-1111-1111-1111-111111111111.cache.json.gz", result);
        }

        [Fact]
        public void ReadCacheFile_FilenameNull_ThrowsArgumentNullException()
        {
            // act; should throw
            Assert.Throws<ArgumentNullException>(() => _sut.ReadCacheFile(null));
        }

        [Fact]
        public void ReadCacheFile_CacheFileDoesNotExist_ReturnsEmptyList()
        {
            // act
            var results = _sut.ReadCacheFile(@"C:\root\DoesNotExist\cache.json.gz");

            // assert
            Assert.NotNull(results);
            Assert.Empty(results);
        }

        [Fact]
        public void ReadCacheFile_CacheFileExists_Returns_ExpectedList()
        {
            // arrange
            const string folder = @"C:\root\subFolder";
            const string cacheFile = @"C:\root\subFolder\11111111-1111-1111-1111-111111111111.cache.json.gz";

            var measurements = CreateMeasurements();
            var data = SerialiseData(measurements);

            _mockfileSystem.AddDirectory(folder);
            _mockfileSystem.AddFile(cacheFile, new MockFileData(data));

            // act
            var results = _sut.ReadCacheFile(cacheFile);

            // assert
            Assert.NotNull(results);
            Assert.Equal(2, results.Count);

            Assert.Equal(measurements[0].Time, results[0].Time);
            Assert.Equal(measurements[0].Temperature, results[0].Temperature);

            Assert.Equal(measurements[1].Time, results[1].Time);
            Assert.Equal(measurements[1].Temperature, results[1].Temperature);
        }

        [Fact]
        public void WriteCacheFile_FilenameNull_ThrowsArgumentNullException()
        {
            // act; should throw
            Assert.Throws<ArgumentNullException>(() => _sut.WriteCacheFile(null, new List<Measurement>()));
        }

        [Fact]
        public void WriteCacheFile_DataNull_ThrowsArgumentNullException()
        {
            // act; should throw
            Assert.Throws<ArgumentNullException>(() => _sut.WriteCacheFile(@"C:\root\subFolder\11111111-1111-1111-1111-111111111111.cache.json.gz", null));
        }

        [Fact]
        public void WriteCacheFile_WritesDataToFile()
        {
            // arrange
            const string folder = @"C:\root\subFolder";
            const string cacheFile = @"C:\root\subFolder\11111111-1111-1111-1111-111111111111.cache.json.gz";

            var measurements = CreateMeasurements();

            _mockfileSystem.AddDirectory(folder);

            // act
            _sut.WriteCacheFile(cacheFile, measurements);

            // assert
            Assert.True(_mockfileSystem.File.Exists(cacheFile), "Cache file was not created");
            Assert.True(_mockfileSystem.FileInfo.New(cacheFile).Length > 0, "Cache file should not be empty");
        }

        [Fact]
        public void WriteCacheFile_FileExists_OverwritesDataInFile()
        {
            // arrange
            const string folder = @"C:\root\subFolder";
            const string cacheFile = @"C:\root\subFolder\11111111-1111-1111-1111-111111111111.cache.json.gz";

            var measurements = CreateMeasurements();

            _mockfileSystem.AddDirectory(folder);
            _mockfileSystem.File.WriteAllText(cacheFile, "Dummy data");

            // act
            _sut.WriteCacheFile(cacheFile, measurements);

            // assert
            Assert.True(_mockfileSystem.File.Exists(cacheFile), "Cache file was not created");
            Assert.True(_mockfileSystem.FileInfo.New(cacheFile).Length > 0, "Cache file should not be empty");
        }

        [Fact]
        public void WriteCacheFile_FolderDoesNotExist_ThrowsException()
        {
            // arrange
            const string folder = @"C:\root\subFolder";
            const string cacheFile = @"C:\root\subFolder\11111111-1111-1111-1111-111111111111.cache.json.gz";

            var measurements = CreateMeasurements();

            Assert.False(_mockfileSystem.Directory.Exists(folder));

            // act
            Assert.Throws<DirectoryNotFoundException>(() => _sut.WriteCacheFile(cacheFile, measurements));
        }

        private static byte[] SerialiseData(List<Measurement> measurements)
        {
            var serializer = new JsonSerializer()
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                Formatting = Formatting.Indented
            };

            using (var stream = new MemoryStream())
            {
                using (var compressor = new GZipStream(stream, CompressionMode.Compress))
                {
                    using (var tw = new StreamWriter(compressor, Encoding.UTF8))
                    {
                        using (var writer = new JsonTextWriter(tw))
                        {
                            serializer.Serialize(writer, measurements);
                        }
                    }
                }

                return stream.ToArray();
            }
        }

        private List<Measurement> CreateMeasurements()
        {
            return new List<Measurement>
            {
                new Measurement(new DateTime(2025, 1, 1, 12, 0, 0), -2d, 45d, -1, 2.95d),
                new Measurement(new DateTime(2025, 1, 1, 12, 15, 0), 2.5d, 45d, -1, 2.9d)
            };
        }
    }
}
