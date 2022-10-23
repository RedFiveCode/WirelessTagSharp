using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using WirelessTagClientApp.ViewModels;

namespace WirelessTagClientApp.Test.ViewModel
{
    [TestClass]
    public class AboutViewModelTest
    {
        [TestMethod]
        public void Class_Should_Implement_INotifyPropertyChanged()
        {
            // act
            var target = new AboutViewModel();

            // assert
            Assert.IsInstanceOfType(target, typeof(INotifyPropertyChanged));
        }

        [TestMethod]
        public void Ctor_Should_Initialise_Properties_To_Expected_Values()
        {
            // act
            var target = new AboutViewModel();

            // assert
            Assert.AreEqual(String.Empty, target.AppName);
            Assert.AreEqual(String.Empty, target.Version);
            Assert.AreEqual(String.Empty, target.Copyright);
            Assert.AreEqual(String.Empty, target.CompanyURL);
            Assert.AreEqual(String.Empty, target.Credits);
            Assert.AreEqual(DateTime.MinValue, target.BuildDate);

            Assert.IsNotNull(target.NavigateCommand);
        }

        [TestMethod]
        public void Initialise_Should_Set_Properties_To_Expected_Values()
        {
            // arrange
            var target = new AboutViewModel();

            // act
            target.Initialise();

            // assert
            Assert.AreNotEqual(String.Empty, target.AppName);
            Assert.AreNotEqual(String.Empty, target.Version);
            Assert.AreNotEqual(String.Empty, target.Copyright);
            Assert.AreNotEqual(String.Empty, target.CompanyURL);
            Assert.AreNotEqual(String.Empty, target.Credits);
            Assert.AreNotEqual(DateTime.MinValue, target.BuildDate);
        }
    }
}
