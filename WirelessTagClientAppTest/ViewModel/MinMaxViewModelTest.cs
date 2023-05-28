using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WirelessTagClientApp.ViewModels;
using WirelessTagClientAppTest.TestHelpers;
using WirelessTagClientLib;
using WirelessTagClientLib.DTO;

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
            Assert.IsNotNull(target.CopyCommand);
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
