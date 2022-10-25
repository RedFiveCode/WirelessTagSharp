using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using WirelessTagClientApp.ViewModels;
using WirelessTagClientAppTest.TestHelpers;

namespace WirelessTagClientApp.Test.ViewModel
{
    [TestClass]
    public class TagViewModelTest
    {
        [TestMethod]
        public void Class_Should_Implement_INotifyPropertyChanged()
        {
            // act
            var target = new TagViewModel();

            // assert
            Assert.IsInstanceOfType(target, typeof(INotifyPropertyChanged));
        }

        [TestMethod]
        public void Ctor_Should_Initialise_Properties_To_Expected_Values()
        {
            // act
            var target = new TagViewModel();

            // assert
            Assert.AreEqual(TagViewModel.ViewMode.Temperature, target.Mode);
            Assert.AreEqual(0, target.Id);
            Assert.AreEqual(String.Empty, target.Name);
            Assert.AreEqual(String.Empty, target.Description);
            Assert.AreEqual(Guid.Empty, target.Uuid);
            Assert.AreEqual(0d, target.Temperature);
            Assert.AreEqual(32d, target.TemperatureFahrenheit);
            Assert.AreEqual(0d, target.RelativeHumidity);
            Assert.AreEqual(DateTime.MinValue, target.LastCommunication);
            Assert.AreEqual(0, target.SignalStrength);
            Assert.AreEqual(0d, target.BatteryVoltage);
            Assert.AreEqual(0d, target.BatteryRemaining);
            Assert.IsFalse(target.IsHumidityTag);
        }

        [TestMethod]
        public void Mode_Setter_Should_Fire_PropertyChanged_Event()
        {
            // arrange
            var target = new TagViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.Mode = TagViewModel.ViewMode.Humidity;

            // assert
            observer.AssertPropertyChangedEvent("Mode");
        }

        [TestMethod]
        public void Name_Setter_Should_Fire_PropertyChanged_Event()
        {
            // arrange
            var target = new TagViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.Name = "my tag";

            // assert
            observer.AssertPropertyChangedEvent("Name");
        }

        [TestMethod]
        public void Description_Setter_Should_Fire_PropertyChanged_Event()
        {
            // arrange
            var target = new TagViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.Description = "my description";

            // assert
            observer.AssertPropertyChangedEvent("Description");
        }

        [TestMethod]
        public void Id_Setter_Should_Fire_PropertyChanged_Event()
        {
            // arrange
            var target = new TagViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.Id = 42;

            // assert
            observer.AssertPropertyChangedEvent("Id");
        }

        [TestMethod]
        public void Uuid_Setter_Should_Fire_PropertyChanged_Event()
        {
            // arrange
            var target = new TagViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.Uuid = Guid.NewGuid();

            // assert
            observer.AssertPropertyChangedEvent("Uuid");
        }

        [TestMethod]
        public void Temperature_Setter_Should_Fire_PropertyChanged_Events()
        {
            // arrange
            var target = new TagViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.Temperature = 42;

            // assert
            observer.AssertPropertyChangedEvent("Temperature");
            observer.AssertPropertyChangedEvent("TemperatureFahrenheit");
        }

        [TestMethod]
        public void RelativeHumidity_Setter_Should_Fire_PropertyChanged_Event()
        {
            // arrange
            var target = new TagViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.RelativeHumidity = 42;

            // assert
            observer.AssertPropertyChangedEvent("RelativeHumidity");
        }

        [TestMethod]
        public void LastCommunication_Setter_Should_Fire_PropertyChanged_Event()
        {
            // arrange
            var target = new TagViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.LastCommunication = DateTime.Now;

            // assert
            observer.AssertPropertyChangedEvent("LastCommunication");
        }

        [TestMethod]
        public void SignalStrength_Setter_Should_Fire_PropertyChanged_Event()
        {
            // arrange
            var target = new TagViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.SignalStrength = 42;

            // assert
            observer.AssertPropertyChangedEvent("SignalStrength");
        }

        [TestMethod]
        public void BatteryVoltage_Setter_Should_Fire_PropertyChanged_Event()
        {
            // arrange
            var target = new TagViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.BatteryVoltage = 1.5;

            // assert
            observer.AssertPropertyChangedEvent("BatteryVoltage");
        }

        [TestMethod]
        public void BatteryRemaining_Setter_Should_Fire_PropertyChanged_Event()
        {
            // arrange
            var target = new TagViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.BatteryRemaining = 10;

            // assert
            observer.AssertPropertyChangedEvent("BatteryRemaining");
        }

        [TestMethod]
        public void Temperature_Setter_Should_Set_TemperatureFahrenheit_Property()
        {
            // arrange
            var target = new TagViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.Temperature = 100;

            // assert
            Assert.AreEqual(212d, target.TemperatureFahrenheit);
        }

        [TestMethod]
        public void IsHumidityTag_Setter_Should_Fire_PropertyChanged_Event()
        {
            // arrange
            var target = new TagViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.IsHumidityTag = true;

            // assert
            observer.AssertPropertyChangedEvent("IsHumidityTag");
        }
    }
}
