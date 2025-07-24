using Xunit;
using System;
using System.ComponentModel;
using WirelessTagClientApp.ViewModels;

namespace WirelessTagClientApp.Test.ViewModel
{
    public class AboutViewModelTest
    {
        [Fact]
        public void Class_Should_Implement_INotifyPropertyChanged()
        {
            // act
            var target = new AboutViewModel();

            // assert
            Assert.IsAssignableFrom<INotifyPropertyChanged>(target);
        }

        [Fact]
        public void Ctor_Should_Initialise_Properties_To_Expected_Values()
        {
            // act
            var target = new AboutViewModel();

            // assert
            Assert.Equal(String.Empty, target.AppName);
            Assert.Equal(String.Empty, target.Version);
            Assert.Equal(String.Empty, target.Copyright);
            Assert.Equal(String.Empty, target.CompanyURL);
            Assert.Equal(String.Empty, target.Credits);
            Assert.Equal(DateTime.MinValue, target.BuildDate);

            Assert.NotNull(target.NavigateCommand);
        }

        [Fact]
        public void Initialise_Should_Set_Properties_To_Expected_Values()
        {
            // arrange
            var target = new AboutViewModel();

            // act
            target.Initialise();

            // assert
            Assert.NotEqual(String.Empty, target.AppName);
            Assert.NotEqual(String.Empty, target.Version);
            Assert.NotEqual(String.Empty, target.Copyright);
            Assert.NotEqual(String.Empty, target.CompanyURL);
            Assert.NotEqual(String.Empty, target.Credits);
            Assert.NotEqual(DateTime.MinValue, target.BuildDate);
        }
    }
}
