using Xunit;
using Moq;
using System.Windows.Input;
using WirelessTagClientApp.Commands;
using WirelessTagClientApp.Interfaces;

namespace WirelessTagClientApp.Test.Commands
{
    
    public class NavigateHyperlinkCommandTest
    {
        [Fact]
        public void NavigateHyperlinkCommand_Implements_ICommand()
        {
            var target = new NavigateHyperlinkCommand();

            Assert.IsAssignableFrom<ICommand>(target.Command);
        }

        [Fact]
        public void NavigateHyperlinkCommand_Execute_OpensUrl()
        {
            // arrange
            var mock = new Mock<IProcessStarter>();
            mock.Setup(x => x.Start(It.IsAny<string>()));

            var target = new NavigateHyperlinkCommand(mock.Object);

            // act
            target.Command.Execute("https://github.com");

            // assert
            mock.Verify(x => x.Start(It.Is<string>(url => url == "https://github.com")));
        }
    }
}
