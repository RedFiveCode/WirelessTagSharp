using Moq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using WirelessTagClientLib.Client;
using WirelessTagClientLib.DTO;
using Xunit;

namespace WirelessTagClientLib.Test.Client
{
    public class CacheLoaderTest
    {
        private readonly Mock<IWirelessTagAsyncClient> _clientMock;
        private readonly Mock<IFileSystem> _fileSystemMock;
        private readonly MockFileSystem _mockfileSystem;
        private readonly CacheLoader _sut;

        public CacheLoaderTest()
        {
            _clientMock = new Mock<IWirelessTagAsyncClient>();
            _fileSystemMock = new Mock<IFileSystem>();
            _mockfileSystem = new MockFileSystem();

            _clientMock.Setup(x => x.GetTemperatureRawDataAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                       .Returns(Task.FromResult(new List<DTO.Measurement>()
                       {
                           new Measurement(new DateTime(2025, 1, 1), 20d),
                           new Measurement(new DateTime(2025, 1, 2), 21d)
                       }));

            _clientMock.Setup(x => x.GetTagListAsync())
                       .Returns(Task.FromResult(new List<DTO.TagInfo>()
                       {
                            new TagInfo
                            {
                                 SlaveId = 1,
                                 Name = "TestTag",
                                 Uuid = Guid.Parse("11111111-1111-1111-1111-111111111111")
                            }
                       }));

            _sut = new CacheLoader(_clientMock.Object, _mockfileSystem)
            {
                ChunkInterval = TimeSpan.FromDays(5),
                WaitInterval = TimeSpan.FromSeconds(0),
            };
        }

        [Fact]
        public async Task LoadCacheAsync_Calls_GetTagListAsync()
        {
            // arrange
            var tagId = 1;
            var folder = @"C:\root\subFolder";
            var from = new DateTime(2025, 1, 1);
            var to = new DateTime(2025, 1, 31);

            // act
            await _sut.LoadCacheAsync(tagId, folder, from, to);

            // assert 
            _clientMock.Verify(x => x.GetTagListAsync(), Times.Once());
        }

        [Fact]
        public async Task LoadCacheAsync_Calls_GetTemperatureRawDataAsync()
        {
            // arrange
            var tagId = 1;
            var folder = @"C:\root\subFolder";
            var from = new DateTime(2025, 1, 1);
            var to = new DateTime(2025, 1, 31);

            // act
            await _sut.LoadCacheAsync(tagId, folder, from, to);

            // assert 
            _clientMock.Verify(x => x.GetTemperatureRawDataAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.AtLeastOnce());
        }

        [Fact]
        public async Task LoadCacheAsync_TagNotFound_DoesNotCallGetTemperatureRawDataAsync()
        {
            // arrange
            var tagId = 999;
            var folder = @"C:\root\subFolder";
            var from = new DateTime(2025, 1, 1);
            var to = new DateTime(2025, 1, 31);

            // act
            await _sut.LoadCacheAsync(tagId, folder, from, to);

            // assert 
            _clientMock.Verify(x => x.GetTemperatureRawDataAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never());
        }

        [Fact]
        public async Task LoadCacheAsync_WritesToCacheFile()
        {
            // arrange
            var tagId = 1;
            var folder = @"C:\root\subFolder";
            var from = new DateTime(2025, 1, 1);
            var to = new DateTime(2025, 1, 31);

            // act
            await _sut.LoadCacheAsync(tagId, folder, from, to);

            // assert 
            var cacheFile = @"C:\root\subFolder\11111111-1111-1111-1111-111111111111.cache.json";

            Assert.True(_mockfileSystem.File.Exists(cacheFile), "Cache file was not created");
            Assert.True(_mockfileSystem.FileInfo.New(cacheFile).Length > 0, "Cache file should not be empty");
        }

        [Fact]
        public async Task LoadCacheAsync_CreateFolder()
        {
            // arrange
            var tagId = 1;
            var folder = @"C:\root\subFolder";
            var from = new DateTime(2025, 1, 1);
            var to = new DateTime(2025, 1, 31);

            // act
            await _sut.LoadCacheAsync(tagId, folder, from, to);

            // assert 
            Assert.True(_mockfileSystem.Directory.Exists(folder), "Cache folder was not created");
        }

    }
}
