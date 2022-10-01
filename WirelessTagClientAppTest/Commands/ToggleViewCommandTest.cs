using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Windows.Input;
using WirelessTagClientApp.Commands;
using WirelessTagClientApp.ViewModels;

namespace WirelessTagClientApp.Test.Commands
{
    [TestClass]
    public class ToggleViewCommandTest
    {
        [TestMethod]
        public void ToggleViewCommand_Implements_ICommand()
        {
            var target = new ToggleViewCommand();

            Assert.IsInstanceOfType(target.Command, typeof(ICommand));
        }

        [TestMethod]
        public void ToggleViewCommand_SetsViewMode_ForAllTags()
        {
            // arrange
            var target = new ToggleViewCommand();
            var viewModel = new AllTagsViewModel();
            viewModel.Tags.Add(new TagViewModel() { Id = 1, Mode = TagViewModel.ViewMode.Temperature });
            viewModel.Tags.Add(new TagViewModel() { Id = 2, Mode = TagViewModel.ViewMode.Temperature });

            Assert.AreEqual(TagViewModel.ViewMode.Temperature, viewModel.Tags[0].Mode);
            Assert.AreEqual(TagViewModel.ViewMode.Temperature, viewModel.Tags[1].Mode);


            // act
            target.Command.Execute(viewModel);

            // assert
            Assert.AreEqual(TagViewModel.ViewMode.TemperatureF, viewModel.Tags[0].Mode);
            Assert.AreEqual(TagViewModel.ViewMode.TemperatureF, viewModel.Tags[1].Mode);
        }

        [TestMethod]
        public void ToggleViewCommand_SetsNextViewMode_And_WrapsRound()
        {
            // arrange
            var viewModes = (TagViewModel.ViewMode[])Enum.GetValues(typeof(TagViewModel.ViewMode));
            var orderedViewModes = viewModes.ToArray().OrderBy(x => x);
            var first = orderedViewModes.First(); // TagViewModel.ViewMode.Temperature
            var last = orderedViewModes.Last(); // TagViewModel.ViewMode.VerboseDetails


            var tagViewModel = new TagViewModel() { Id = 1, Mode = TagViewModel.ViewMode.Temperature };

            var target = new ToggleViewCommand();
            var viewModel = new AllTagsViewModel();
          
            viewModel.Tags.Add(tagViewModel);

            tagViewModel.Mode = last; // TagViewModel.ViewMode.VerboseDetails

            // act
            target.Command.Execute(viewModel);

            // assert
            Assert.AreEqual(first, tagViewModel.Mode);
        }

        [TestMethod]
        public void ToggleViewCommand_SetsPreviousViewMode_ForAllTags()
        {
            // arrange
            var target = new ToggleViewCommand(ToggleViewCommand.Direction.Previous);
            var viewModel = new AllTagsViewModel();
            viewModel.Tags.Add(new TagViewModel() { Id = 1, Mode = TagViewModel.ViewMode.TemperatureF });
            viewModel.Tags.Add(new TagViewModel() { Id = 2, Mode = TagViewModel.ViewMode.TemperatureF });

            Assert.AreEqual(TagViewModel.ViewMode.TemperatureF, viewModel.Tags[0].Mode);
            Assert.AreEqual(TagViewModel.ViewMode.TemperatureF, viewModel.Tags[1].Mode);


            // act
            target.Command.Execute(viewModel);

            // assert
            Assert.AreEqual(TagViewModel.ViewMode.Temperature, viewModel.Tags[0].Mode);
            Assert.AreEqual(TagViewModel.ViewMode.Temperature, viewModel.Tags[1].Mode);
        }

        [TestMethod]
        public void ToggleViewCommand_SetsPreviousViewMode_And_WrapsRound()
        {
            // arrange
            var viewModes = (TagViewModel.ViewMode[])Enum.GetValues(typeof(TagViewModel.ViewMode));
            var orderedViewModes = viewModes.ToArray().OrderBy(x => x);
            var first = orderedViewModes.First(); // TagViewModel.ViewMode.Temperature
            var last = orderedViewModes.Last(); // TagViewModel.ViewMode.VerboseDetails


            var tagViewModel = new TagViewModel() { Id = 1, Mode = TagViewModel.ViewMode.Temperature };

            var target = new ToggleViewCommand(ToggleViewCommand.Direction.Previous);
            var viewModel = new AllTagsViewModel();

            viewModel.Tags.Add(tagViewModel);

            tagViewModel.Mode = first;

            // act
            target.Command.Execute(viewModel);

            // assert
            Assert.AreEqual(last, tagViewModel.Mode);
        }
    }
}
