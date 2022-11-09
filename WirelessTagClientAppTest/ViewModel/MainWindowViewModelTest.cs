using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using WirelessTagClientApp.ViewModels;
using WirelessTagClientAppTest.TestHelpers;
using WirelessTagClientLib;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientApp.Test.ViewModel
{
    [TestClass]
    public class MainWindowViewModelTest
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
        public void Class_Should_Implement_INotifyPropertyChanged()
        {
            // act
            var target = new MainWindowViewModel();

            // assert
            Assert.IsInstanceOfType(target, typeof(INotifyPropertyChanged));
        }

        [TestMethod]
        public void Ctor_Should_Initialise_Properties_To_Expected_Values()
        {
            // act
            var target = new MainWindowViewModel();

            // assert
            Assert.AreEqual(DateTime.MinValue, target.LastUpdated);
            Assert.IsFalse(target.IsBusy);
            Assert.IsFalse(target.IsError);
            Assert.AreEqual(String.Empty, target.ErrorMessage);

            // commands
            Assert.IsNotNull(target.CloseCommand);
            Assert.IsNotNull(target.RefreshCommand);
            Assert.IsNotNull(target.AboutCommand);
            Assert.IsNotNull(target.SummaryViewCommand);
            Assert.IsNotNull(target.MinMaxViewCommand);

            // view-model(s)
            Assert.AreEqual(MainWindowViewModel.ViewMode.SummaryView, target.Mode);
            Assert.IsNotNull(target.ActiveViewModel);
        }

        [TestMethod]
        public void LastUpdated_Setter_Should_Fire_PropertyChanged_Event()
        {
            // arrange
            var target = new MainWindowViewModel();
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
            var target = new MainWindowViewModel();
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
            var target = new MainWindowViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.IsError = true;

            // assert
            observer.AssertPropertyChangedEvent("IsError");
        }

        [TestMethod]
        public void ErrorMessage_Setter_Should_Fire_PropertyChanged_Events()
        {
            // arrange
            var target = new MainWindowViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.ErrorMessage = "Error message";

            // assert
            observer.AssertPropertyChangedEvent("ErrorMessage");
            observer.AssertPropertyChangedEvent("IsError");
        }

        [TestMethod]
        public void ErrorMessage_Setter_Should_Set_IsError_Property()
        {
            // arrange
            var target = new MainWindowViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.ErrorMessage = "Error message";

            // assert
           Assert.IsTrue(target.IsError);
        }

        [TestMethod]
        public void ErrorMessage_Setter_Should_Reset_IsError_Property()
        {
            // arrange
            var target = new MainWindowViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.ErrorMessage = "";

            // assert
            Assert.IsFalse(target.IsError);
        }

        [TestMethod]
        public void SetError_Should_Set_Depedent_Properties()
        {
            // arrange
            var target = new MainWindowViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.SetError("my error message");

            // assert
            observer.AssertPropertyChangedEvent("IsError");
            observer.AssertPropertyChangedEvent("ErrorMessage");
            observer.AssertPropertyChangedEvent("IsBusy");

            Assert.IsTrue(target.IsError);
            Assert.AreEqual("my error message", target.ErrorMessage);
            Assert.IsFalse(target.IsBusy);
        }

        [TestMethod]
        public void Mode_Setter_Should_Fire_PropertyChanged_Events()
        {
            // arrange
            var target = new MainWindowViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.Mode = MainWindowViewModel.ViewMode.MinMaxView;

            // assert
            observer.AssertPropertyChangedEvent("Mode");
            observer.AssertPropertyChangedEvent("ActiveViewModel");
        }

        [TestMethod]
        public void Mode_Setter_Should_Set_ActiveViewModel()
        {
            // arrange
            var target = new MainWindowViewModel();
            var originalValue = target.ActiveViewModel;

            // act
            target.Mode = MainWindowViewModel.ViewMode.MinMaxView;

            // assert
            Assert.AreNotSame(originalValue, target.ActiveViewModel);
        }

        [TestMethod]
        public void Refresh_Should_Set_IsBusy_Property()
        {
            // arrange
            var mock = CreateAsyncClientMock();
            var target = new MainWindowViewModel(mock.Object, new Options());
            var observer = new PropertyChangedObserver(target);

            // act
            target.Refresh();

            // assert
            observer.AssertPropertyChangedEvent("IsBusy", 2); // should set then reset IsBusy
        }

        [TestMethod]
        public void Refresh_Should_Set_LastUpdated_Property()
        {
            // arrange
            var mock = CreateAsyncClientMock();
            var target = new MainWindowViewModel(mock.Object, new Options());
            var observer = new PropertyChangedObserver(target);

            // act
            target.Refresh();

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
