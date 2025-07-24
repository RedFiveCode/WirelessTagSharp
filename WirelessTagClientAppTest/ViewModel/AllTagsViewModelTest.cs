using Xunit;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WirelessTagClientApp.ViewModels;
using WirelessTagClientAppTest.TestHelpers;

namespace WirelessTagClientApp.Test.ViewModel
{
    
    public class AllTagsViewModelTest
    {
        [Fact]
        public void Class_Should_Implement_INotifyPropertyChanged()
        {
            // act
            var target = new AllTagsViewModel();

            // assert
            Assert.IsAssignableFrom<INotifyPropertyChanged>(target);
        }

        [Fact]
        public void Ctor_Should_Initialise_Properties_To_Expected_Values()
        {
            // arrange
            var parentViewModel = new MainWindowViewModel();

            // act
            var target = new AllTagsViewModel(parentViewModel);

            // assert
            Assert.NotNull(target.Tags);
            Assert.Empty(target.Tags);
            Assert.Equal(DateTime.MinValue, target.LastUpdated);

            Assert.NotNull(target.ToggleNextViewCommand);
            Assert.NotNull(target.TogglePreviousViewCommand);
            Assert.NotNull(target.CopyCommand);
            Assert.NotNull(target.RefreshCommand);
            Assert.Same(parentViewModel, target.ParentViewModel);
        }

        [Fact]
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

        [Fact]
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
