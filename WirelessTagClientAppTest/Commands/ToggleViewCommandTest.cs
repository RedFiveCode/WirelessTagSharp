using Xunit;
using System;
using System.Linq;
using System.Windows.Input;
using WirelessTagClientApp.Commands;
using WirelessTagClientApp.ViewModels;

namespace WirelessTagClientApp.Test.Commands
{
    
    public class ToggleViewCommandTest
    {
        [Fact]
        public void ToggleViewCommand_Implements_ICommand()
        {
            var target = new ToggleViewCommand();

            Assert.IsAssignableFrom<ICommand>(target.Command);
        }

        [Fact]
        public void ToggleViewCommand_SetsViewMode()
        {
            // arrange
            var target = new ToggleViewCommand();
            var viewModel = new AllTagsViewModel()
            {
                Mode = AllTagsViewModel.ViewMode.Temperature
            };

            // act
            target.Command.Execute(viewModel);

            // assert
            Assert.Equal(AllTagsViewModel.ViewMode.TemperatureF, viewModel.Mode);
        }

        [Fact]
        public void ToggleViewCommand_SetsNextViewMode_And_WrapsRound()
        {
            // arrange
            var viewModes = (AllTagsViewModel.ViewMode[])Enum.GetValues(typeof(AllTagsViewModel.ViewMode));
            var orderedViewModes = viewModes.ToArray().OrderBy(x => x);
            var first = orderedViewModes.First(); // ViewMode.Temperature
            var last = orderedViewModes.Last(); // ViewMode.VerboseDetails

            var target = new ToggleViewCommand();
            var viewModel = new AllTagsViewModel()
            {
                Mode = last
            };
          
            // act
            target.Command.Execute(viewModel);

            // assert
            Assert.Equal(first, viewModel.Mode);
        }

        [Fact]
        public void ToggleViewCommand_SetsPreviousViewMode_ForAllTags()
        {
            // arrange
            var viewModes = (AllTagsViewModel.ViewMode[])Enum.GetValues(typeof(AllTagsViewModel.ViewMode));
            var orderedViewModes = viewModes.ToArray().OrderBy(x => x);
            var first = orderedViewModes.First(); // ViewMode.Temperature
            var last = orderedViewModes.Last(); // ViewMode.VerboseDetails

            var target = new ToggleViewCommand(ToggleViewCommand.Direction.Previous);
            var viewModel = new AllTagsViewModel()
            {
                Mode = first // Temperature
            };

            // act
            target.Command.Execute(viewModel);

            // assert
            Assert.Equal(last, viewModel.Mode); // VerboseDetails
        }

        [Fact]
        public void ToggleViewCommand_SetsPreviousViewMode_And_WrapsRound()
        {
            // arrange
            var viewModes = (AllTagsViewModel.ViewMode[])Enum.GetValues(typeof(AllTagsViewModel.ViewMode));
            var orderedViewModes = viewModes.ToArray().OrderBy(x => x);
            var first = orderedViewModes.First(); // TagViewModel.ViewMode.Temperature
            var last = orderedViewModes.Last(); // TagViewModel.ViewMode.VerboseDetails

            var target = new ToggleViewCommand(ToggleViewCommand.Direction.Previous);
            var viewModel = new AllTagsViewModel();


            // act
            target.Command.Execute(viewModel);

            // assert
            Assert.Equal(last, viewModel.Mode);
        }
    }
}
