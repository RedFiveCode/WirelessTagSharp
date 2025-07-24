using Xunit;
using Moq;
using System.Windows;
using System.Windows.Input;
using WirelessTagClientApp.Commands;
using WirelessTagClientApp.Interfaces;

namespace WirelessTagClientApp.Test.Commands
{
    
    public class AboutCommandTest
    {
        [Fact]
        public void AboutCommand_Implements_ICommand()
        {
            var target = new AboutCommand();

            Assert.IsAssignableFrom<ICommand>(target.Command);
        }

        [Fact]
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
