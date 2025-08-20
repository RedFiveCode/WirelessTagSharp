using Moq;
using System;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Reflection;
using Xunit;

namespace WirelessTagClientApp.Test
{
    public class OptionsExtensionTest
    {
        [Fact]
        public void ResolveCacheFolder_OptionsNull_ReturnsNull()
        {
            // arrange
            var options = new Options();
            var fileSystemMock = new Mock<IFileSystem>();

            var target = new OptionsExtensions(fileSystemMock.Object);

            // act
            var result = target.ResolveCacheFolder(null);

            // assert
            Assert.Null(result);

        }

        [Fact]
        public void ResolveCacheFolder_FolderNull_ReturnsNull()
        {
            // arrange
            var options = new Options()
            {
                CacheFolder = null
            };

            var fileSystemMock = new Mock<IFileSystem>();

            var target = new OptionsExtensions(fileSystemMock.Object);

            // act
            var result = target.ResolveCacheFolder(null);

            // assert
            Assert.Null(result);
        }

        [Fact]
        public void ResolveCacheFolder_RootedFolder_ReturnsOriginalFolder()
        {
            // arrange
            var options = new Options()
            {
                CacheFolder = @"C:\rootFolder\subFolder"
            };

            var fileSystemMock = new Mock<IFileSystem>();

            var target = new OptionsExtensions(fileSystemMock.Object);

            // act
            var result = target.ResolveCacheFolder(options);

            // assert
            Assert.Equal(@"C:\rootFolder\subFolder", result);
        }

        [Fact]
        public void ResolveCacheFolder_RelativePath_FolderExists_ReturnsRootedFolder()
        {
            // arrange
            var options = new Options()
            {
                CacheFolder = @"..\cacheFolder"
            };

            var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var expectedFolder = Path.GetDirectoryName(assemblyFolder); // parent directory
            expectedFolder = Path.Combine(expectedFolder, @"cacheFolder");

            var fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.Setup(x => x.Directory.Exists(It.IsAny<string>()))
                          .Returns(true); // simulate that the directory exists


            var target = new OptionsExtensions(fileSystemMock.Object);

            // act
            var result = target.ResolveCacheFolder(options);

            // assert

            Assert.StartsWith(@"C:\Users\", result); // check if it starts with a rooted path
            Assert.EndsWith(@"cacheFolder", result);
        }

        [Fact]
        public void ResolveCacheFolder_RelativePath_FolderNotExists_ReturnsRootedFolder()
        {
            // arrange
            var options = new Options()
            {
                CacheFolder = @"..\cacheFolder"
            };

            var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var expectedFolder = Path.GetDirectoryName(assemblyFolder); // parent directory
            expectedFolder = Path.Combine(expectedFolder, @"cacheFolder");

            var fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.Setup(x => x.Directory.Exists(It.IsAny<string>()))
                          .Returns(false); // simulate that the directory does not exist

            var target = new OptionsExtensions(fileSystemMock.Object);

            // act
            var result = target.ResolveCacheFolder(options);

            // assert
            Assert.Null(result); // since the directory does not exist, it should return null
        }
    }
}
