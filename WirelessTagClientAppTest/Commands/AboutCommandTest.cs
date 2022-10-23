using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Windows;
using System.Windows.Input;
using WirelessTagClientApp.Commands;
using WirelessTagClientApp.Interfaces;

namespace WirelessTagClientApp.Test.Commands
{
    [TestClass]
    public class AboutCommandTest
    {
        [TestMethod]
        public void AboutCommand_Implements_ICommand()
        {
            var target = new AboutCommand();

            Assert.IsInstanceOfType(target.Command, typeof(ICommand));
        }

        [TestMethod]
        public void AboutCommand_Execute_OpensDialogBox()
        {
            // arrange
            var mock = CreateMock();
            var target = new AboutCommand(mock.Object);
            Window parentWindow = null;

            // act
            target.Command.Execute(parentWindow);

            // assert
            mock.Verify(x => x.ShowDialog(It.IsAny<Window>()), Times.Once);
        }

        private Mock<IDialogService> CreateMock()
        {
            var mock = new Mock<IDialogService>();
            mock.Setup(x => x.ShowDialog(It.IsAny<Window>())).Returns(true);

            return mock;
        }
    }
}
