using System;
using System.IO;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using Newtonsoft.Json;
using WirelessTagClientLib.Client;
using WirelessTagClientLib.DTO;
using Xunit;
using System.IO.Compression;
using System.Text;

namespace WirelessTagClientLib.Test.Client
{
    public class CacheReaderTest
    {
        private readonly MockFileSystem _mockfileSystem;
        private readonly TagInfo _tagInfo;
        private readonly CacheReader _sut;

        public CacheReaderTest()
        {
            _mockfileSystem = new MockFileSystem();
            _mockfileSystem.AddDirectory(@"C:\root");

            _tagInfo = new TagInfo
            {
                SlaveId = 1,
                Name = "TestTag",
                Uuid = Guid.Parse("11111111-1111-1111-1111-111111111111")
            };

            _sut = new CacheReader(_mockfileSystem);
        }

        [Fact]
        public void ReadCache_FolderNull_ThrowsArgumentNullException()
        {
            // act; should throw
            Assert.Throws<ArgumentNullException>(() => _sut.ReadCache(null, _tagInfo));
        }

        [Fact]
        public void ReadCache_TagNull_ThrowsArgumentNullException()
        {
            // act; should throw
            Assert.Throws<ArgumentNullException>(() => _sut.ReadCache(@"C:\root\subFolder", null));
        }

        [Fact]
        public void ReadCache_FolderNotExist_ThrowsDirectoryNotFoundExceptionn()
        {
            // act; should throw
            Assert.Throws<DirectoryNotFoundException>(() => _sut.ReadCache(@"C:\root\DoesNotExist", _tagInfo));
        }

        [Fact]
        public void ReadCache_CacheFileExists_Returns_ExpectedList()
        {
            // arrange
            var measurements = new List<Measurement>
            {
                new Measurement(new DateTime(2025, 1, 1, 12, 0, 0), -2d, 45d, -1, 2.95d),
                new Measurement(new DateTime(2025, 1, 1, 12, 15, 0), 2.5d, 45d, -1, 2.9d)
            };

            var data = SerialiseData(measurements);

            _mockfileSystem.AddDirectory(@"C:\root\subFolder");
            _mockfileSystem.AddFile(@"C:\root\subFolder\11111111-1111-1111-1111-111111111111.cache.json.gz", new MockFileData(data));

            // act
            var results = _sut.ReadCache(@"C:\root\subFolder", _tagInfo);

            // assert
            Assert.NotNull(results);
            Assert.Equal(2, results.Count);

            Assert.Equal(measurements[0].Time, results[0].Time);
            Assert.Equal(measurements[0].Temperature, results[0].Temperature);

            Assert.Equal(measurements[1].Time, results[1].Time);
            Assert.Equal(measurements[1].Temperature, results[1].Temperature);
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

        [Fact]
        public void ReadCache_CacheFileDoesNotExist_Returns_EmptyList()        
        {
            // arrange
            _mockfileSystem.AddDirectory(@"C:\root\subFolder");

            // act
            var results = _sut.ReadCache(@"C:\root\subFolder", _tagInfo);

            // assert
            Assert.NotNull(results);
            Assert.Empty(results);
        }
    }
}
