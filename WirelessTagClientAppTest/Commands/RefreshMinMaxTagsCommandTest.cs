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
    public class RefreshMinMaxTagsCommandTest
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
            var target = new RefreshMinMaxTagsCommand(clientMock.Object, new Options());

            Assert.IsInstanceOfType(target.Command, typeof(ICommand));
        }

        [TestMethod]
        public void CanExecute_Returns_True()
        {
            // arrange
            var clientMock = CreateAsyncClientMock();
            var target = new RefreshMinMaxTagsCommand(clientMock.Object, new Options());

            var viewModel = new MinMaxViewModel();

            // act
            var result = target.Command.CanExecute(viewModel);

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Execute_Should_Call_WirelessTagClient_LoginAsync()
        {
            // arrange
            var clientMock = CreateAsyncClientMock();
            var target = new RefreshMinMaxTagsCommand(clientMock.Object, new Options());
            var viewModel = new MinMaxViewModel();

            // act
            await target.ExecuteAsync(viewModel);

            // assert
            clientMock.Verify(x => x.LoginAsync(It.IsAny<string>(), It.IsAny<string>()));
        }

        [TestMethod]
        public async Task Execute_Should_Call_WirelessTagClient_GetTemperatureRawDataAsync()
        {
            // arrange
            var clientMock = CreateAsyncClientMock();
            var target = new RefreshMinMaxTagsCommand(clientMock.Object, new Options());
            var viewModel = new MinMaxViewModel();

            // act
            await target.ExecuteAsync(viewModel);

            // assert
            clientMock.Verify(x => x.GetTemperatureRawDataAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()));
        }

        [TestMethod]
        public async Task Execute_Should_Update_Data_Property()
        {
            // arrange
            var clientMock = CreateAsyncClientMock();
            var target = new RefreshMinMaxTagsCommand(clientMock.Object, new Options());
            var viewModel = new MinMaxViewModel();

            bool onCollectionChanged = false;
            viewModel.Data.CollectionChanged += (sender, args) => { onCollectionChanged = true; };

            // act
            await target.ExecuteAsync(viewModel);

            // assert
            Assert.IsTrue(onCollectionChanged);
        }

        [TestMethod]
        public async Task Execute_Sets_LastUpdated_Property()
        {
            // arrange
            var clientMock = CreateAsyncClientMock();
            var target = new RefreshMinMaxTagsCommand(clientMock.Object, new Options());
            var viewModel = new MinMaxViewModel();

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

            clientMock.Setup(x => x.GetTemperatureRawDataAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                      .Callback(() => Console.WriteLine("Mocked GetTemperatureRawDataAsync callback"))
                      .ReturnsAsync(new List<TemperatureDataPoint>()
                      {  
                          new TemperatureDataPoint(DateTime.Today.Date, 10d)
                      }); ;

            return clientMock;
        }
    }
}
