using Xunit;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WirelessTagClientApp.ViewModels;
using WirelessTagClientAppTest.TestHelpers;

namespace WirelessTagClientApp.Test.ViewModel
{
    
    public class MinMaxViewModelTest
    {
        [Fact]
        public void Class_Should_Implement_INotifyPropertyChanged()
        {
            // act
            var target = new MinMaxViewModel();

            // assert
            Assert.IsAssignableFrom<INotifyPropertyChanged>(target);
        }

        [Fact]
        public void Ctor_Should_Initialise_Properties_To_Expected_Values()
        {
            // arrange
            var parent = new MainWindowViewModel();

            // act
            var target = new MinMaxViewModel(parent);

            // assert
            Assert.NotNull(target.Data);
            Assert.Equal(DateTime.MinValue, target.LastUpdated);
            Assert.Equal(TemperatureUnits.Celsius, target.TemperatureUnits);
            Assert.NotNull(target.CopyCommand);
            Assert.NotNull(target.CopyRawDataCommand);
            Assert.NotNull(target.ToggleTemperatureUnitsCommand);
            Assert.NotNull(target.RefreshCommand);
            Assert.NotNull(target.RawDataCache);
            Assert.Same(parent, target.ParentViewModel);
        }

        [Fact]
        public void Data_Setter_Should_Fire_PropertyChanged_Event()
        {
            // arrange
            var target = new MinMaxViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.Data = new ObservableCollection<MinMaxMeasurementViewModel>();

            // assert
            observer.AssertPropertyChangedEvent("Data");
        }

        [Fact]
        public void TemperatureUnits_Setter_Should_Fire_PropertyChanged_Events()
        {
            // arrange
            var target = new MinMaxViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.TemperatureUnits = TemperatureUnits.Farenheit;

            // assert
            observer.AssertPropertyChangedEvent("TemperatureUnits");
            observer.AssertPropertyChangedEvent("IsTemperatureCelsius");
            observer.AssertPropertyChangedEvent("IsTemperatureFahrenheit");
        }

        [Fact]
        public void TemperatureUnits_SetterFarenheit_Should_SetAssociatedProperties()
        {
            // arrange
            var target = new MinMaxViewModel();

            // act
            target.TemperatureUnits = TemperatureUnits.Farenheit;

            // assert
            Assert.False(target.IsTemperatureCelsius);
            Assert.True(target.IsTemperatureFahrenheit);
        }

        [Fact]
        public void TemperatureUnits_SetterCelsius_Should_SetAssociatedProperties()
        {
            // arrange
            var target = new MinMaxViewModel();

            // act
            target.TemperatureUnits = TemperatureUnits.Celsius;

            // assert
            Assert.True(target.IsTemperatureCelsius);
            Assert.False(target.IsTemperatureFahrenheit);
        }

        [Fact]
        public void LastUpdated_Setter_Should_Fire_PropertyChanged_Event()
        {
            // arrange
            var target = new MinMaxViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.LastUpdated = DateTime.Now;

            // assert
            observer.AssertPropertyChangedEvent("LastUpdated");
        }
    }
}
