﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Windows.Input;
using WirelessTagClientApp.Commands;
using WirelessTagClientApp.Common;
using WirelessTagClientApp.ViewModels;

namespace WirelessTagClientApp.Test.Commands
{
    /// <summary>
    /// Unit tests for the <see cref="DelegatedCommand"/> class.
    /// </summary>
    [TestClass]
    public class DelegatedCommandTest
    {
        [TestMethod]
        public void DelegatedCommand_Implements_ICommand()
        {
            var target = new DelegatedCommand();

            Assert.IsInstanceOfType(target.Command, typeof(ICommand));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Register_Null_ThrowsArgumentNullException()
        {
            var target = new DelegatedCommand();

            target.Register(ViewModels.MainWindowViewModel.ViewMode.MinMaxView, null);
        }

        [TestMethod]
        public void CanExecute_NullParameter_Returns_False()
        {
            // arrange
            var target = new DelegatedCommand();

            // act
            var result = target.Command.CanExecute(null);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanExecute_NoCommandsRegistered_Returns_False()
        {
            // arrange
            var target = new DelegatedCommand();

            // don't register any commands target.Register(...)

            var viewModel = new MainWindowViewModel();
            viewModel.Mode = MainWindowViewModel.ViewMode.SummaryView;

            // act
            var result = target.Command.CanExecute(viewModel);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanExecute_CommandNotRegistered_Returns_False()
        {
            // arrange
            var target = new DelegatedCommand();
            var summaryViewCommand = CreateMockedCommand();

            // only register any command for one mode
            target.Register(ViewModels.MainWindowViewModel.ViewMode.SummaryView, summaryViewCommand.Object);
            // but not for MinMaxView

            var viewModel = new MainWindowViewModel();
            viewModel.Mode = MainWindowViewModel.ViewMode.MinMaxView;

            // act
            var result = target.Command.CanExecute(viewModel);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanExecute_CallsDelegatedCanExecute_WithActiveViewModel()
        {
            // arrange
            var target = new DelegatedCommand();
            var summaryViewCommand = CreateMockedCommand();
            var minMaxViewCommand = CreateMockedCommand();

            target.Register(ViewModels.MainWindowViewModel.ViewMode.SummaryView, summaryViewCommand.Object);
            target.Register(ViewModels.MainWindowViewModel.ViewMode.MinMaxView, minMaxViewCommand.Object);

            var viewModel = new MainWindowViewModel();
            viewModel.Mode = MainWindowViewModel.ViewMode.SummaryView;

            // act
            var result = target.Command.CanExecute(viewModel);

            // assert
            Assert.IsTrue(result);

            // should call the expected command for the active view to do the CanExecute work
            summaryViewCommand.Verify(x => x.CanExecute(It.Is<ViewModelBase>(vm => vm == viewModel.ActiveViewModel)), Times.Once);

            // and not call the other command for the other (inactive) view
            minMaxViewCommand.Verify(x => x.CanExecute(It.IsAny<object>()), Times.Never());
        }

        [TestMethod]
        public void Execute_CallsDelegatedExecute_WithActiveViewModel()
        {
            // arrange
            var target = new DelegatedCommand();
            var summaryViewCommand = CreateMockedCommand();
            var minMaxViewCommand = CreateMockedCommand();

            target.Register(ViewModels.MainWindowViewModel.ViewMode.SummaryView, summaryViewCommand.Object);
            target.Register(ViewModels.MainWindowViewModel.ViewMode.MinMaxView, minMaxViewCommand.Object);

            var viewModel = new MainWindowViewModel();
            viewModel.Mode = MainWindowViewModel.ViewMode.SummaryView;

            // act
            target.Command.Execute(viewModel);

            // assert
            // should call the expected command for the active view to do the Execute work
            summaryViewCommand.Verify(x => x.Execute(It.Is<ViewModelBase>(vm => vm == viewModel.ActiveViewModel)), Times.Once);

            // and not call the other command for the other (inactive) view
            minMaxViewCommand.Verify(x => x.Execute(It.IsAny<object>()), Times.Never());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Execute_NoCommandsRegistered_Throws_InvalidOperationException()
        {
            // arrange
            var target = new DelegatedCommand();

            // don't register any commands target.Register(...)

            var viewModel = new MainWindowViewModel();
            viewModel.Mode = MainWindowViewModel.ViewMode.SummaryView;

            // act - should throw
            target.Command.Execute(viewModel);
        }

        private Mock<ICommand> CreateMockedCommand()
        {
            var mock = new Mock<ICommand>();

            mock.Setup(x => x.CanExecute(It.IsAny<object>())).Returns(true);
            mock.Setup(x => x.Execute(It.IsAny<object>()));

            return mock;
        }

    }

    
}
