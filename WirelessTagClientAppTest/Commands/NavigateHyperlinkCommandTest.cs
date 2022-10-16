using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Windows.Input;
using WirelessTagClientApp.Commands;
using WirelessTagClientApp.Interfaces;

namespace WirelessTagClientApp.Test.Commands
{
    [TestClass]
    public class NavigateHyperlinkCommandTest
    {
        [TestMethod]
        public void NavigateHyperlinkCommand_Implements_ICommand()
        {
            var target = new NavigateHyperlinkCommand();

            Assert.IsInstanceOfType(target.Command, typeof(ICommand));
        }

        [TestMethod]
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
