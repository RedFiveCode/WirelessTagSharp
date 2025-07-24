using Xunit;
using System.Windows.Input;
using WirelessTagClientApp.Commands;

namespace WirelessTagClientApp.Test.Commands
{
    /// <summary>
    /// Unit tests for the <see cref="RefreshViewCommand"/> command.
    /// </summary>
    
    public class RefreshViewCommandTest
    {
        [Fact]
        public void RefreshViewCommand_Implements_ICommand()
        {
            var target = new RefreshViewCommand();

            Assert.IsAssignableFrom<ICommand>(target.Command);
        }
    }
}
