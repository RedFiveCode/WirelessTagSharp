using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WirelessTagClientApp.ViewModels;
using WirelessTagClientAppTest.TestHelpers;

namespace WirelessTagClientApp.Test.ViewModel
{
    [TestClass]
    public class AllTagsViewModelTest
    {
        [TestMethod]
        public void Class_Should_Implement_INotifyPropertyChanged()
        {
            // act
            var target = new AllTagsViewModel();

            // assert
            Assert.IsInstanceOfType(target, typeof(INotifyPropertyChanged));
        }

        [TestMethod]
        public void Ctor_Should_Initialise_Properties_To_Expected_Values()
        {
            // arrange
            var parentViewModel = new MainWindowViewModel();

            // act
            var target = new AllTagsViewModel(parentViewModel);

            // assert
            Assert.IsNotNull(target.Tags);
            Assert.AreEqual(0, target.Tags.Count);
            Assert.AreEqual(DateTime.MinValue, target.LastUpdated);

            Assert.IsNotNull(target.ToggleNextViewCommand);
            Assert.IsNotNull(target.TogglePreviousViewCommand);
            Assert.IsNotNull(target.CopyCommand);
            Assert.IsNotNull(target.RefreshCommand);
            Assert.AreSame(parentViewModel, target.ParentViewModel);
        }

        [TestMethod]
        public void Tags_Setter_Should_Fire_PropertyChanged_Event()
        {
            // arrange
            var target = new AllTagsViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.Tags = new ObservableCollection<TagViewModel>();

            // assert
            observer.AssertPropertyChangedEvent("Tags");
        }

        [TestMethod]
        public void LastUpdated_Setter_Should_Fire_PropertyChanged_Event()
        {
            // arrange
            var target = new AllTagsViewModel();
            var observer = new PropertyChangedObserver(target);

            // act
            target.LastUpdated = DateTime.Now;

            // assert
            observer.AssertPropertyChangedEvent("LastUpdated");
        }
    }
}
