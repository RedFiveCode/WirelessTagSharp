using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using WirelessTagClientApp.Commands;
using WirelessTagClientApp.ViewModels;
using WirelessTagClientAppTest.TestHelpers;
using WirelessTagClientLib;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientApp.Test.Commands
{
    [TestClass]
    public class RefreshAllTagsCommandAsyncTest
    {
        [TestMethod]
        public void Command_Implements_ICommand()
        {
            var clientMock = CreateAsyncClientMock();
            var options = new Options();

            var target = new RefreshAllTagsCommand(clientMock.Object, options);

            Assert.IsInstanceOfType(target.Command, typeof(ICommand));
        }

        [TestMethod]
        public void CanExecute_Returns_True()
        {
            // arrange
            var clientMock = CreateAsyncClientMock();
            var options = new Options();

            var target = new RefreshAllTagsCommand(clientMock.Object, options);

            var viewModel = new MainWindowViewModel();

            // act
            var result = target.Command.CanExecute(viewModel);

            // assert
            var innerViewModel = viewModel.ActiveViewModel;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Execute_RefreshesListOfTagsInViewModel()
        {
            // arrange
            var clientMock = CreateAsyncClientMock();
            var options = new Options();

            var target = new RefreshAllTagsCommand(clientMock.Object, options);

            var viewModel = new MainWindowViewModel();

            // act - await async operations returning
            await target.ExecuteAsync(viewModel);

            // assert
            var innerViewModel = viewModel.ActiveViewModel as AllTagsViewModel;
            Assert.IsNotNull(innerViewModel);
            Assert.IsNotNull(innerViewModel.Tags);
            Assert.AreEqual(1, innerViewModel.Tags.Count);
            Assert.AreEqual(1, innerViewModel.Tags[0].Id);
        }

        [TestMethod]
        public async Task Execute_Sets_ViewModel_IsBusy()
        {
            // arrange
            var clientMock = CreateAsyncClientMock();
            var options = new Options();

            var target = new RefreshAllTagsCommand(clientMock.Object, options);

            var viewModel = new MainWindowViewModel();
            var observer = new PropertyChangedObserver(viewModel);

            Assert.IsFalse(viewModel.IsBusy);

            // act - await async operations returning
            var task = target.ExecuteAsync(viewModel);
            Assert.IsTrue(viewModel.IsBusy);

            await task;

            // assert
            observer.AssertPropertyChangedEvent("IsBusy");
        }

        [TestMethod]
        public async Task Execute_Error_Sets_ViewModel_Error()
        {
            // arrange
            var clientMock = new Mock<IWirelessTagAsyncClient>();
            clientMock.Setup(x => x.GetTagListAsync())
                      .Throws(new InvalidOperationException("test exception"));

            var options = new Options();

            var target = new RefreshAllTagsCommand(clientMock.Object, options);

            var viewModel = new MainWindowViewModel();
            var observer = new PropertyChangedObserver(viewModel);

            Assert.IsFalse(viewModel.IsError);
            Assert.AreEqual(String.Empty, viewModel.ErrorMessage);

            // act - await async operations returning
            await target.ExecuteAsync(viewModel);

            // assert
            observer.AssertPropertyChangedEvent("IsError");
            observer.AssertPropertyChangedEvent("ErrorMessage");

            Assert.IsTrue(viewModel.IsError);
            Assert.AreNotEqual(String.Empty, viewModel.ErrorMessage);
        }

        [TestMethod]
        public async Task Execute_Calls_Client_GetTagList()
        {
            // arrange
            var clientMock = new Mock<IWirelessTagAsyncClient>();

            bool getTagListCalled = false;
            clientMock.Setup(x => x.GetTagListAsync())
                .Callback(() => getTagListCalled = true)
                .ReturnsAsync(new List<TagInfo>() { new TagInfo() { SlaveId = 1 } });

            var options = new Options();

            var target = new RefreshAllTagsCommand(clientMock.Object, options);

            var viewModel = new MainWindowViewModel();

            // act - await async operations returning
            await target.ExecuteAsync(viewModel);

            // assert
            Assert.IsTrue(getTagListCalled);
        }


        private Mock<IWirelessTagAsyncClient> CreateAsyncClientMock()
        {
            var clientMock = new Mock<IWirelessTagAsyncClient>();

            clientMock.Setup(x => x.LoginAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Callback(() => Console.WriteLine("Mocked LoginAsync callback"))
                .ReturnsAsync(true);

            clientMock.Setup(x => x.GetTagListAsync())
                .Callback(() => Console.WriteLine("Mocked GetTagListAsync callback"))
                .ReturnsAsync(new List<TagInfo>() { new TagInfo() { SlaveId = 1 } });

            return clientMock;
        }
    }
}
