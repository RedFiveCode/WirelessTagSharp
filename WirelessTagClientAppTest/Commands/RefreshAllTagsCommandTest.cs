using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
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
    public class RefreshAllTagsCommandTest
    {
        [TestInitialize]
        public void TestSetup()
        {
            // Ensure we have a SynchronizationContext for task continuations in the view-model;
            // WPF has this by default, but unit tests do not, otherwise we get an InvalidOperationException
            // "The current SynchronizationContext may not be used as a TaskScheduler"
            // See https://stackoverflow.com/questions/8245926/the-current-synchronizationcontext-may-not-be-used-as-a-taskscheduler
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
        }

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

            var viewModel = new AllTagsViewModel();

            // act - await async operations returning
            await target.ExecuteAsync(viewModel);

            // assert
            Assert.IsNotNull(viewModel.Tags);
            Assert.AreEqual(1, viewModel.Tags.Count);
            Assert.AreEqual(1, viewModel.Tags[0].Id);
        }

        [TestMethod]
        public async Task Execute_AlreadyHaveSomeData_RefreshesListOfTagsInViewModel_AndMaintainsPreviousTagViewMode()
        {
            // arrange
            var clientMock = CreateAsyncClientMock();
            var options = new Options();

            var target = new RefreshAllTagsCommand(clientMock.Object, options);

            var viewModel = new AllTagsViewModel();

            // add some tags to the view model
            var tagViewModel = new TagViewModel()
            {
                Id = 1,
                Name = "my tag",
                Mode = TagViewModel.ViewMode.VerboseDetails
            };

            viewModel.Tags.Add(tagViewModel);

            var previousViewMode = tagViewModel.Mode;

            // act - await async operations returning
            await target.ExecuteAsync(viewModel);

            // assert
            Assert.IsNotNull(viewModel.Tags);
            Assert.AreEqual(1, viewModel.Tags.Count);
            Assert.AreEqual(1, viewModel.Tags[0].Id);
            Assert.AreEqual(previousViewMode, viewModel.Tags[0].Mode);
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

            var viewModel = new AllTagsViewModel();

            // act - await async operations returning
            await target.ExecuteAsync(viewModel);

            // assert
            Assert.IsTrue(getTagListCalled);
        }

        [TestMethod]
        public async Task Execute_Sets_LastUpdated_Property()
        {
            // arrange
            var clientMock = CreateAsyncClientMock();
            var options = new Options();

            var target = new RefreshAllTagsCommand(clientMock.Object, options);

            var viewModel = new AllTagsViewModel();

            var observer = new PropertyChangedObserver(viewModel);

            // act - await async operations returning
            await target.ExecuteAsync(viewModel);

            // assert
            observer.AssertPropertyChangedEvent("LastUpdated");
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
