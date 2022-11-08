using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Input;
using WirelessTagClientApp.Commands;

namespace WirelessTagClientApp.Test.Commands
{
    /// <summary>
    /// Unit tests for the <see cref="RefreshViewCommand"/> command.
    /// </summary>
    [TestClass]
    public class RefreshViewCommandTest
    {
        [TestMethod]
        public void RefreshViewCommand_Implements_ICommand()
        {
            var target = new RefreshViewCommand();

            Assert.IsInstanceOfType(target.Command, typeof(ICommand));
        }
    }
}
