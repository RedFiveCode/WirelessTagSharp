using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using WirelessTagClientApp.ViewModels;
using WirelessTagClientAppTest.TestHelpers;

namespace WirelessTagClientApp.Test.ViewModel
{
    [TestClass]
    public class MainWindowViewModelTest
    {
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

            // view-model(s)
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
    }
}
