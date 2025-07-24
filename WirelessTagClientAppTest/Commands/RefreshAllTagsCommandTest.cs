using Xunit;
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
using System.Linq;

namespace WirelessTagClientApp.Test.Commands
{
    
    public class RefreshAllTagsCommandTest
    {
        
        public void TestSetup()
        {
            // Ensure we have a SynchronizationContext for task continuations in the view-model;
            // WPF has this by default, but unit tests do not, otherwise we get an InvalidOperationException
            // "The current SynchronizationContext may not be used as a TaskScheduler"
            // See https://stackoverflow.com/questions/8245926/the-current-synchronizationcontext-may-not-be-used-as-a-taskscheduler
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
        }

        [Fact]
        public void Command_Implements_ICommand()
        {
            var clientMock = CreateAsyncClientMock();
            var options = new Options();

            var target = new RefreshAllTagsCommand(clientMock.Object, options);

            Assert.IsAssignableFrom<ICommand>(target.Command);
        }

        [Fact]
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
            Assert.True(result);
        }

        [Fact]
        public async Task Execute_RefreshesListOfTagsInViewModel()
        {
            // arrange
            var clientMock = CreateAsyncClientMock();
            var options = new Options();

            var target = new RefreshAllTagsCommand(clientMock.Object, options);

            var parentViewModel = new MainWindowViewModel();
            var viewModel = new AllTagsViewModel(parentViewModel);

            // act - await async operations returning
            await target.ExecuteAsync(viewModel);

            // assert
           Assert.NotNull(viewModel.Tags);
            Assert.Equal(1, viewModel.Tags.Count);
            Assert.Equal(1, viewModel.Tags[0].Id);
        }

        [Fact]
        public async Task Execute_AlreadyHaveSomeData_RefreshesListOfTagsInViewModel_AndMaintainsPreviousTagViewMode()
        {
            // arrange
            var clientMock = CreateAsyncClientMock();
            var options = new Options();

            var target = new RefreshAllTagsCommand(clientMock.Object, options);

            var parentViewModel = new MainWindowViewModel();
            var viewModel = new AllTagsViewModel(parentViewModel);

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
           Assert.NotNull(viewModel.Tags);
            Assert.Equal(1, viewModel.Tags.Count);
            Assert.Equal(1, viewModel.Tags[0].Id);
            Assert.Equal(previousViewMode, viewModel.Tags[0].Mode);
        }

        [Fact]
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

                        var parentViewModel = new MainWindowViewModel();
            var viewModel = new AllTagsViewModel(parentViewModel);

            // act - await async operations returning
            await target.ExecuteAsync(viewModel);

            // assert
            Assert.True(getTagListCalled);
        }

        [Fact]
        public async Task Execute_Sets_LastUpdated_Property_OnParentViewModel()
        {
            // arrange
            var clientMock = CreateAsyncClientMock();
            var options = new Options();

            var target = new RefreshAllTagsCommand(clientMock.Object, options);

            var parentViewModel = new MainWindowViewModel();

            var viewModel = new AllTagsViewModel(parentViewModel);

            var observer = new PropertyChangedObserver(parentViewModel);

            // act - await async operations returning
            await target.ExecuteAsync(viewModel);

            // assert
            observer.AssertPropertyChangedEvent("LastUpdated");
        }

        [Fact]
        public async Task Execute_SetsIsBusy_Property_OnParentViewModel()
        {
            // arrange
            var clientMock = CreateAsyncClientMock();
            var options = new Options();

            var target = new RefreshAllTagsCommand(clientMock.Object, options);

            var parentViewModel = new MainWindowViewModel();

            var viewModel = new AllTagsViewModel(parentViewModel);

            var observer = new PropertyChangedObserver(parentViewModel);

            // act - await async operations returning
            await target.ExecuteAsync(viewModel);

            // assert
            observer.AssertPropertyChangedEvent("IsBusy", 2); // set then reset
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
