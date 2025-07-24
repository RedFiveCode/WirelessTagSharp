using Xunit;
using System;
using System.Windows.Input;
using WirelessTagClientApp.Commands;
using WirelessTagClientApp.ViewModels;

namespace WirelessTagClientApp.Test.Commands
{
    /// <summary>
    /// Unit tests for the <see cref="ChangeViewCommand"/> command.
    /// </summary>
    
    public class ChangeViewCommandTest
    {
        [Fact]
        public void ChangeViewCommand_Implements_ICommand()
        {
            var target = new ChangeViewCommand(ViewModels.MainWindowViewModel.ViewMode.SummaryView);

            Assert.IsAssignableFrom<ICommand>(target.Command);
        }

        [Fact]
        public void ChangeViewCommand_Sets_MainWindowViewModel_Mode()
        {
            // arrange
            var viewModel = new MainWindowViewModel();
            var target = new ChangeViewCommand(ViewModels.MainWindowViewModel.ViewMode.MinMaxView);

            Assert.NotEqual(MainWindowViewModel.ViewMode.MinMaxView, viewModel.Mode);

            // act
            target.Command.Execute(viewModel);

            // assert
            Assert.Equal(MainWindowViewModel.ViewMode.MinMaxView, viewModel.Mode);
        }

        [Fact]
        public void ChangeViewCommand_Updates_MainWindowViewModel_LastUpdated()
        {
            // arrange
            var target = new ChangeViewCommand(ViewModels.MainWindowViewModel.ViewMode.MinMaxView);
            var viewModel = new MainWindowViewModel();

            // update lastUpdated property for each child's view-model
            UpdateChildViewModelTimestamp(viewModel, MainWindowViewModel.ViewMode.SummaryView, new DateTime(2023, 5, 28));
            UpdateChildViewModelTimestamp(viewModel, MainWindowViewModel.ViewMode.MinMaxView, new DateTime(2023, 9, 29));

            Assert.NotEqual(MainWindowViewModel.ViewMode.MinMaxView, viewModel.Mode);

            // act
            target.Command.Execute(viewModel); // change from SummaryView to MinMaxView

            // assert
            Assert.Equal(new DateTime(2023, 9, 29), viewModel.LastUpdated);
        }

        private void UpdateChildViewModelTimestamp(MainWindowViewModel viewModel, MainWindowViewModel.ViewMode mode, DateTime timestamp)
        {
            var originalMode = viewModel.Mode;

            viewModel.Mode = mode;
            if (viewModel.ActiveViewModel is MinMaxViewModel)
            {
                var minMaxViewModel = viewModel.ActiveViewModel as MinMaxViewModel;
                minMaxViewModel.LastUpdated = timestamp;
            }
            else if (viewModel.ActiveViewModel is AllTagsViewModel)
            {
                var allTagsViewModel = viewModel.ActiveViewModel as AllTagsViewModel;
                allTagsViewModel.LastUpdated = timestamp;
            }

            viewModel.Mode = originalMode;
        }
    }
}
