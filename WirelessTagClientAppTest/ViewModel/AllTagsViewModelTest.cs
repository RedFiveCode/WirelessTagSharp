using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using WirelessTagClientApp.ViewModels;
using WirelessTagClientAppTest.TestHelpers;
using WirelessTagClientLib;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientAppTest.ViewModel
{
    [TestClass]
    public class AllTagsViewModelTest
    {
        [TestMethod]
        public void Class_Should_Implement_INotifyPropertyChanged()
        {
            // act
            var target = new AllTagsViewModel();

            // assert
            Assert.IsInstanceOfType(target, typeof(INotifyPropertyChanged));
        }

        [TestMethod]
        public void Ctor_Should_Initialise_Properties_To_Expected_Values()
        {
            // act
            var target = new AllTagsViewModel();

            // assert
            Assert.IsNotNull(target.Tags);
            Assert.AreEqual(0, target.Tags.Count);

            Assert.AreEqual(DateTime.MinValue, target.LastUpdated);

            Assert.IsFalse(target.IsBusy);
            Assert.IsFalse(target.IsError);
            Assert.AreEqual(String.Empty, target.ErrorMessage);

            // commands
            Assert.IsNotNull(target.RefreshCommand);
        }

        [TestMethod]
        public void Tags_Setter_Should_Fire_PropertyChanged_Event()
        {
            // arrange
            var target = new AllTagsViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.Tags = new ObservableCollection<TagViewModel>();

            // assert
            observer.AssertPropertyChangedEvent("Tags");
        }

        [TestMethod]
        public void LastUpdated_Setter_Should_Fire_PropertyChanged_Event()
        {
            // arrange
            var target = new AllTagsViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.LastUpdated = DateTime.Now;

            // assert
            observer.AssertPropertyChangedEvent("LastUpdated");
        }

        [TestMethod]
        public void IsBusy_Setter_Should_Fire_PropertyChanged_Event()
        {
            // arrange
            var target = new AllTagsViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.IsBusy = true;

            // assert
            observer.AssertPropertyChangedEvent("IsBusy");
        }

        [TestMethod]
        public void IsError_Setter_Should_Fire_PropertyChanged_Event()
        {
            // arrange
            var target = new AllTagsViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.IsError = true;

            // assert
            observer.AssertPropertyChangedEvent("IsError");
        }

        [TestMethod]
        public void ErrorMessage_Setter_Should_Fire_PropertyChanged_Event()
        {
            // arrange
            var target = new AllTagsViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.ErrorMessage = "Error message";

            // assert
            observer.AssertPropertyChangedEvent("ErrorMessage");
        }

        [TestMethod]
        public void Refresh_Should_Call_WirelessTagClient_LoginAsync()
        {
            // arrange
            var clientMock = CreateAsyncClientMock();

            var target = new AllTagsViewModel(clientMock.Object);

            // act
            target.Refresh();

            // assert
            clientMock.Verify(x => x.LoginAsync(It.IsAny<string>(), It.IsAny<string>()));
        }

        [TestMethod]
        public void Refresh_Should_Call_WirelessTagClient_GetTagListAsync()
        {
            // arrange
            var clientMock = CreateAsyncClientMock();

            var target = new AllTagsViewModel(clientMock.Object);

            // act
            target.Refresh();

            // assert
            clientMock.Verify(x => x.GetTagListAsync());
        }

        private Mock<IWirelessTagClient> CreateClientMock()
        {
            var clientMock = new Mock<IWirelessTagClient>();

            return clientMock;
        }

        private Mock<IWirelessTagAsyncClient> CreateAsyncClientMock()
        {
            var clientMock = new Mock<IWirelessTagAsyncClient>();

            //clientMock.Setup(x => x.LoginAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true));
            clientMock.Setup(x => x.LoginAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
            clientMock.Setup(x => x.GetTagListAsync()).ReturnsAsync(new List<TagInfo>());

            return clientMock;
        }
    }
}
