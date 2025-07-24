using Xunit;
using System;
using System.ComponentModel;
using WirelessTagClientApp.ViewModels;
using WirelessTagClientAppTest.TestHelpers;

namespace WirelessTagClientApp.Test.ViewModel
{
    
    public class TagViewModelTest
    {
        [Fact]
        public void Class_Should_Implement_INotifyPropertyChanged()
        {
            // act
            var target = new TagViewModel();

            // assert
            Assert.IsAssignableFrom<INotifyPropertyChanged>(target);
        }

        [Fact]
        public void Ctor_Should_Initialise_Properties_To_Expected_Values()
        {
            // act
            var target = new TagViewModel();

            // assert
            Assert.Equal(TagViewModel.ViewMode.Temperature, target.Mode);
            Assert.Equal(0, target.Id);
            Assert.Equal(String.Empty, target.Name);
            Assert.Equal(String.Empty, target.Description);
            Assert.Equal(Guid.Empty, target.Uuid);
            Assert.Equal(0d, target.Temperature);
            Assert.Equal(32d, target.TemperatureFahrenheit);
            Assert.Equal(0d, target.RelativeHumidity);
            Assert.Equal(DateTime.MinValue, target.LastCommunication);
            Assert.Equal(0, target.SignalStrength);
            Assert.Equal(0d, target.BatteryVoltage);
            Assert.Equal(0d, target.BatteryRemaining);
            Assert.False(target.IsHumidityTag);
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
        public void Temperature_Setter_Should_Set_TemperatureFahrenheit_Property()
        {
            // arrange
            var target = new TagViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.Temperature = 100;

            // assert
            Assert.Equal(212d, target.TemperatureFahrenheit);
        }

        [Fact]
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
