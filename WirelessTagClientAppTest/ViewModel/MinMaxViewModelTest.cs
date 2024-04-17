using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WirelessTagClientApp.ViewModels;
using WirelessTagClientAppTest.TestHelpers;

namespace WirelessTagClientApp.Test.ViewModel
{
    [TestClass]
    public class MinMaxViewModelTest
    {
        [TestMethod]
        public void Class_Should_Implement_INotifyPropertyChanged()
        {
            // act
            var target = new MinMaxViewModel();

            // assert
            Assert.IsInstanceOfType(target, typeof(INotifyPropertyChanged));
        }

        [TestMethod]
        public void Ctor_Should_Initialise_Properties_To_Expected_Values()
        {
            // act
            var target = new MinMaxViewModel();

            // assert
            Assert.IsNotNull(target.Data);
            Assert.AreEqual(DateTime.MinValue, target.LastUpdated);
            Assert.AreEqual(TemperatureUnits.Celsius, target.TemperatureUnits);
            Assert.IsNotNull(target.CopyCommand);
            Assert.IsNotNull(target.ToggleTemperatureUnitsCommand);
            Assert.IsNotNull(target.RawDataCache);
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void TemperatureUnits_SetterFarenheit_Should_SetAssociatedProperties()
        {
            // arrange
            var target = new MinMaxViewModel();

            // act
            target.TemperatureUnits = TemperatureUnits.Farenheit;

            // assert
            Assert.IsFalse(target.IsTemperatureCelsius);
            Assert.IsTrue(target.IsTemperatureFahrenheit);
        }

        [TestMethod]
        public void TemperatureUnits_SetterCelsius_Should_SetAssociatedProperties()
        {
            // arrange
            var target = new MinMaxViewModel();

            // act
            target.TemperatureUnits = TemperatureUnits.Celsius;

            // assert
            Assert.IsTrue(target.IsTemperatureCelsius);
            Assert.IsFalse(target.IsTemperatureFahrenheit);
        }

        [TestMethod]
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
