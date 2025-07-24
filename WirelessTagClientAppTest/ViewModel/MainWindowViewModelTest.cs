using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using WirelessTagClientApp.ViewModels;
using WirelessTagClientAppTest.TestHelpers;
using WirelessTagClientLib;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientApp.Test.ViewModel
{
    
    public class MainWindowViewModelTest
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
        public void Class_Should_Implement_INotifyPropertyChanged()
        {
            // act
            var target = new MainWindowViewModel();

            // assert
            Assert.IsAssignableFrom<INotifyPropertyChanged>(target);
        }

        [Fact]
        public void Ctor_Should_Initialise_Properties_To_Expected_Values()
        {
            // act
            var target = new MainWindowViewModel();

            // assert
            Assert.Equal(DateTime.MinValue, target.LastUpdated);
            Assert.False(target.IsBusy);
            Assert.False(target.IsError);
            Assert.Equal(String.Empty, target.ErrorMessage);

            // commands
           Assert.NotNull(target.CloseCommand);
           Assert.NotNull(target.RefreshCommand);
           Assert.NotNull(target.AboutCommand);
           Assert.NotNull(target.SummaryViewCommand);
           Assert.NotNull(target.MinMaxViewCommand);
           Assert.NotNull(target.CopyCommand);
           Assert.NotNull(target.CopyRawDataCommand);
           Assert.NotNull(target.ToggleUnitsCommand);

            // view-model(s)
            Assert.Equal(MainWindowViewModel.ViewMode.SummaryView, target.Mode);
           Assert.NotNull(target.ActiveViewModel);
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
        public void ErrorMessage_Setter_Should_Set_IsError_Property()
        {
            // arrange
            var target = new MainWindowViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.ErrorMessage = "Error message";

            // assert
           Assert.True(target.IsError);
        }

        [Fact]
        public void ErrorMessage_Setter_Should_Reset_IsError_Property()
        {
            // arrange
            var target = new MainWindowViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.ErrorMessage = "";

            // assert
            Assert.False(target.IsError);
        }

        [Fact]
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

            Assert.True(target.IsError);
            Assert.Equal("my error message", target.ErrorMessage);
            Assert.False(target.IsBusy);
        }

        [Fact]
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

        [Fact]
        public void Mode_Setter_Should_Set_ActiveViewModel()
        {
            // arrange
            var target = new MainWindowViewModel();
            var originalValue = target.ActiveViewModel;

            // act
            target.Mode = MainWindowViewModel.ViewMode.MinMaxView;

            // assert
            Assert.NotSame(originalValue, target.ActiveViewModel);
        }

        [Fact]
        public async Task Refresh_SummaryView_Should_Set_IsBusy_Property()
        {
            // arrange
            var mock = CreateAsyncClientMock();
            var target = new MainWindowViewModel(mock.Object, new Options());
            target.Mode = MainWindowViewModel.ViewMode.SummaryView;
            var observer = new PropertyChangedObserver(target);

            // act
            await target.Refresh();

            // assert
            observer.AssertPropertyChangedEvent("IsBusy", 2); // should set then reset IsBusy
        }

        [Fact]
        public async Task Refresh_SummaryView_Should_Set_LastUpdated_Property()
        {
            // arrange
            var mock = CreateAsyncClientMock();
            var target = new MainWindowViewModel(mock.Object, new Options());
            target.Mode = MainWindowViewModel.ViewMode.SummaryView;
            var observer = new PropertyChangedObserver(target);

            // act
            await target.Refresh();

            // assert
            observer.AssertPropertyChangedEvent("LastUpdated");
        }

        [Fact]
        public async Task Refresh_MinMaxView_Should_Set_IsBusy_Property()
        {
            // arrange
            var mock = CreateAsyncClientMock();
            var target = new MainWindowViewModel(mock.Object, new Options());
            target.Mode = MainWindowViewModel.ViewMode.MinMaxView;
            var observer = new PropertyChangedObserver(target);

            // act
            await target.Refresh();

            // assert
            observer.AssertPropertyChangedEvent("IsBusy", 2); // should set then reset IsBusy
        }

        [Fact]
        public async Task Refresh_MinMaxView_Should_Set_LastUpdated_Property()
        {
            // arrange
            var mock = CreateAsyncClientMock();
            var target = new MainWindowViewModel(mock.Object, new Options());
            target.Mode = MainWindowViewModel.ViewMode.MinMaxView;
            var observer = new PropertyChangedObserver(target);

            // act
            await target.Refresh();

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
                      .ReturnsAsync(new List<Measurement>()
                      {
                          new Measurement(DateTime.Today.Date, 10d)
                      }); ;

            return clientMock;
        }
    }
}
