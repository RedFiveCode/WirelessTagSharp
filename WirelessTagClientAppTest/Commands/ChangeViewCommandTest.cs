using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Input;
using WirelessTagClientApp.Commands;
using WirelessTagClientApp.ViewModels;

namespace WirelessTagClientApp.Test.Commands
{
    /// <summary>
    /// Unit tests for the <see cref="ChangeViewCommand"/> command.
    /// </summary>
    [TestClass]
    public class ChangeViewCommandTest
    {
        [TestMethod]
        public void ChangeViewCommand_Implements_ICommand()
        {
            var target = new ChangeViewCommand(ViewModels.MainWindowViewModel.ViewMode.SummaryView);

            Assert.IsInstanceOfType(target.Command, typeof(ICommand));
        }

        [TestMethod]
        public void ChangeViewCommand_Sets_MainWindowViewModel_Mode()
        {
            // arrange
            var viewModel = new MainWindowViewModel();
            var target = new ChangeViewCommand(ViewModels.MainWindowViewModel.ViewMode.MinMaxView);

            Assert.AreNotEqual(MainWindowViewModel.ViewMode.MinMaxView, viewModel.Mode);

            // act
            target.Command.Execute(viewModel);

            // assert
            Assert.AreEqual(MainWindowViewModel.ViewMode.MinMaxView, viewModel.Mode);
        }
    }
}
