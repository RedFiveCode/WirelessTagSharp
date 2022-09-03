using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void ToggleViewCommand_SetsViewMode()
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
    }
}
